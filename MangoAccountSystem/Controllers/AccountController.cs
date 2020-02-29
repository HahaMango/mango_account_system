using System;
using System.Threading.Tasks;
using IdentityServer4.Services;
using MangoAccountSystem.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace MangoAccountSystem.Controllers
{
	/// <summary>
	/// 跟登陆，登出，注册有关的控制器
	/// </summary>
	public class AccountController : UserHelperController
	{
		/// <summary>
		/// Identity登陆管理器
		/// </summary>
		private readonly SignInManager<MangoUser> _signInManager;

		/// <summary>
		/// Identity用户管理器
		/// </summary>
		private readonly UserManager<MangoUser> _userManager;

		/// <summary>
		/// Identity角色管理器
		/// </summary>
		private readonly RoleManager<MangoUserRole> _roleManager;

		/// <summary>
		/// IdentityServer4的交互服务
		/// </summary>
		private readonly IIdentityServerInteractionService _interaction;

		/// <summary>
		/// 授权方案服务
		/// </summary>
		private readonly IAuthenticationSchemeProvider _schemeProvider;

		/// <summary>
		/// 构造函数
		/// </summary>
		/// <param name="signInManager"></param>
		/// <param name="userManager"></param>
		/// <param name="roleManager"></param>
		/// <param name="authenticationSchemeProvider"></param>
		/// <param name="identityServerInteractionService"></param>
		public AccountController(
			SignInManager<MangoUser> signInManager,
			UserManager<MangoUser> userManager, RoleManager<MangoUserRole> roleManager,
			IAuthenticationSchemeProvider authenticationSchemeProvider,
			IIdentityServerInteractionService identityServerInteractionService)
		{
			_signInManager = signInManager;
			_userManager = userManager;
			_roleManager = roleManager;
			_interaction = identityServerInteractionService;
			_schemeProvider = authenticationSchemeProvider;
		}

		/// <summary>
		/// 登陆接口
		/// </summary>
		/// <param name="returnUrl"></param>
		/// <returns></returns>
		[HttpGet]
		public async Task<IActionResult> Login(string returnUrl = null)
		{
			if (returnUrl == null)
			{
				returnUrl = "/";
			}

			LoginInputModels loginInputModels = await GetLoginViewModelsAsync(returnUrl);

			return View(loginInputModels);
		}

		/// <summary>
		/// 登陆表单提交入口
		/// </summary>
		/// <param name="loginInputModels"></param>
		/// <returns></returns>
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Login([Bind("UserName,Password,OnlyLocalLogin,ReturnUrl")]LoginInputModels loginInputModels)
		{
			if (loginInputModels == null)
			{
				throw new ArgumentNullException(nameof(loginInputModels));
			}

			if (!ModelState.IsValid)
			{
				return View(loginInputModels);
			}

			if (await _userManager.FindByNameAsync(loginInputModels.UserName) == null)
			{
				ModelState.AddModelError("UserName", "该用户名不存在");
				return View(loginInputModels);
			}

			var success = await _signInManager.PasswordSignInAsync(loginInputModels.UserName, loginInputModels.Password, false, false);
			if (!success.Succeeded)
			{
				ModelState.AddModelError("Password", "密码不正确");
				return View(loginInputModels);
			}
			var mangouser = await _userManager.FindByNameAsync(loginInputModels.UserName);
			mangouser.LastLoginDate = DateTime.Now;
			await _userManager.UpdateAsync(mangouser);

			return Redirect(loginInputModels.ReturnUrl);
		}

		/// <summary>
		/// 注册入口
		/// </summary>
		/// <returns></returns>
		[HttpGet]
		public IActionResult SignUp()
		{
			return View();
		}

		/// <summary>
		/// 注册表单提交入口
		/// </summary>
		/// <param name="signUpInputModel"></param>
		/// <returns></returns>
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> SignUp([Bind("UserName,Email,Password,PasswordConfirm,IsAgree")]SignUpInputModel signUpInputModel)
		{
			if (signUpInputModel == null)
			{
				throw new ArgumentNullException(nameof(signUpInputModel));
			}

			if (!ModelState.IsValid)
			{
				return View(signUpInputModel);
			}

			MangoUser mangoUser = new MangoUser
			{
				UserName = signUpInputModel.UserName,
				Email = signUpInputModel.Email
			};

			if (await _userManager.FindByNameAsync(mangoUser.UserName) != null)
			{
				ModelState.AddModelError("UserName", "该用户名已被注册");
				return View(signUpInputModel);
			}

			var flag = await _userManager.CreateAsync(mangoUser, signUpInputModel.Password);
			mangoUser = await _userManager.FindByNameAsync(mangoUser.UserName);
			await _userManager.AddToRoleAsync(mangoUser, "USER");

			if (!flag.Succeeded)
			{
				ViewData["Message"] = "Registration error occurred!";
			}
			else
			{
				ViewData["Message"] = "Registration Successful!";
			}
			return View("ResultPage");
		}

		/// <summary>
		/// 登出入口
		/// </summary>
		/// <param name="logoutid"></param>
		/// <returns></returns>
		[HttpGet]
		public async Task<IActionResult> Logout(string logoutid)
		{
			LoggedViewModel vm;
			ViewData["IsAuthenticated"] = false;
			ViewData["UserName"] = "";
			if (string.IsNullOrEmpty(logoutid))
			{
				vm = new LoggedViewModel();
			}
			else
			{
				vm = await GetLoggedViewModelsAsync(logoutid);
			}

			if (User?.Identity.IsAuthenticated == true)
			{
				await _signInManager.SignOutAsync();
			}

			return View(vm);
		}

		/// <summary>
		/// 根据returnUrl获取登陆信息，OAuth2.0
		/// </summary>
		/// <param name="returnUrl"></param>
		/// <returns></returns>
		private async Task<LoginInputModels> GetLoginViewModelsAsync(string returnUrl)
		{
			var context = await _interaction.GetAuthorizationContextAsync(returnUrl);

			if (Url.IsLocalUrl(returnUrl) == false && _interaction.IsValidReturnUrl(returnUrl) == false)
			{
				throw new System.Exception("invalid return URL");
			}

			LoginInputModels loginInputModels = new LoginInputModels
			{
				ReturnUrl = returnUrl
			};

			var a = await _schemeProvider.GetAllSchemesAsync();
			if (context != null || context.IdP != null)
			{
				//bool onlyLocalLogin = context.IdP == IdentityServerConstants.LocalIdentityProvider;
				loginInputModels.OnlyLocalLogin = true;
			}

			return loginInputModels;
		}

		/// <summary>
		/// 根据logoutId获取登陆信息，OAuth2.0
		/// </summary>
		/// <param name="logoutId"></param>
		/// <returns></returns>
		private async Task<LoggedViewModel> GetLoggedViewModelsAsync(string logoutId)
		{
			var logoutContext = await _interaction.GetLogoutContextAsync(logoutId);

			LoggedViewModel loggedViewModel = new LoggedViewModel();

			if (logoutContext == null)
			{
				return loggedViewModel;
			}
			else
			{
				if (string.IsNullOrEmpty(logoutContext.PostLogoutRedirectUri))
				{
					throw new InvalidOperationException($"该登陆id没有登出返回链接，Id：{logoutId}，ClientName：{logoutContext.ClientName}");
				}
				loggedViewModel.LogoutReturnUrl = logoutContext.PostLogoutRedirectUri;
			}

			return loggedViewModel;
		}
	}
}

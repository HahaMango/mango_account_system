using IdentityServer4.Services;
using MangoAccountSystem.Dao;
using MangoAccountSystem.Helper;
using MangoAccountSystem.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Threading.Tasks;

namespace MangoAccountSystem.Controllers
{
	/// <summary>
	/// 第三方登陆控制器
	/// </summary>
	public class ExternalController : UserHelperController
	{
		/// <summary>
		/// IdentityServer 交互服务
		/// </summary>
		private readonly IIdentityServerInteractionService _interaction;

		/// <summary>
		/// Identity登陆管理器
		/// </summary>
		private readonly SignInManager<MangoUser> _signInManager;

		/// <summary>
		/// Identity用户管理器
		/// </summary>
		private readonly UserManager<MangoUser> _userManager;

		/// <summary>
		/// 当前请求数据库上下文的事务对象
		/// </summary>
		private readonly Transaction _transaction;

		/// <summary>
		/// 构造函数
		/// </summary>
		/// <param name="signInManager"></param>
		/// <param name="identityServerInteractionService"></param>
		/// <param name="userManager"></param>
		public ExternalController(
			SignInManager<MangoUser> signInManager,
			IIdentityServerInteractionService identityServerInteractionService,
			UserManager<MangoUser> userManager,
			Transaction transaction)
		{
			_signInManager = signInManager;
			_interaction = identityServerInteractionService;
			_userManager = userManager;
			_transaction = transaction;
		}

		/// <summary>
		/// 第三方授权后的回调入口，包含新用户注册逻辑。
		/// </summary>
		/// <param name="returnUrl"></param>
		/// <returns></returns>
		[HttpGet]
		public async Task<IActionResult> Callback(string returnUrl)
		{
			var info = await _signInManager.GetExternalLoginInfoAsync();

			if (info == null)
			{
				return RedirectToAction("Login", "Account", returnUrl);
			}

			ClaimsPrincipal externalUser = info.Principal;

			string providerkey = info.ProviderKey;
			string provider = info.LoginProvider;

			var result = await _signInManager.ExternalLoginSignInAsync(provider, providerkey, false, false);
			if (result.Succeeded)
			{
				return Redirect(returnUrl);
			}

			string userName = externalUser.Identity.Name;
			//第三方授权用户不一定有邮箱
			string email = externalUser.FindFirst(ClaimTypes.Email)?.Value;

			using (var trans = await _transaction.BeginTransactionAsync())
			{
				try
				{
					MangoUser mangoUser;
					//当用户存在邮箱时，判断用户数据库中该邮箱是否已在数据库中，在的话则直接关联该用户
					//否则创建一个新用户
					if (email != null)
					{
						mangoUser = await _userManager.FindByEmailAsync(email);
						if (mangoUser != null)
						{
							var addLoginFlag = await _userManager.AddLoginAsync(mangoUser, info);

							if (addLoginFlag.Succeeded)
							{
								await _signInManager.SignInAsync(mangoUser, false, provider);
								return Redirect(returnUrl);
							}
						}
					}

					mangoUser = new MangoUser
					{
						UserName = userName,
						Email = email
					};
					while (await _userManager.FindByNameAsync(mangoUser.UserName) != null)
					{
						mangoUser.UserName += RandomString.NextString(1);
					}

					var createUserFlag = await _userManager.CreateAsync(mangoUser);

					mangoUser = await _userManager.FindByNameAsync(mangoUser.UserName);

					await _userManager.AddToRoleAsync(mangoUser, "USER");

					if (createUserFlag.Succeeded)
					{
						var addLoginFlag = await _userManager.AddLoginAsync(mangoUser, info);

						if (addLoginFlag.Succeeded)
						{
							await _signInManager.SignInAsync(mangoUser, false, provider);

							return Redirect(returnUrl);
						}
					}

					trans.Commit();
				}
				catch
				{
					trans.Rollback();
					throw;
				}
			}

			ViewData["Message"] = $"{provider}:External Login error occurred!";
			return View("ResultPage");
		}

		/// <summary>
		/// 发起第三方登陆跳转
		/// </summary>
		/// <param name="returnUrl"></param>
		/// <param name="provider"></param>
		/// <returns></returns>
		[HttpGet]
		public IActionResult Challenge(string returnUrl, string provider)
		{
			if (Url.IsLocalUrl(returnUrl) == false && _interaction.IsValidReturnUrl(returnUrl) == false)
			{
				throw new System.Exception("invalid return URL");
			}

			var redirectUrl = Url.Action(new Microsoft.AspNetCore.Mvc.Routing.UrlActionContext
			{
				Controller = "External",
				Action = "Callback",
				Values = new
				{
					returnUrl
				}
			});
			var properties = _signInManager.ConfigureExternalAuthenticationProperties(provider, redirectUrl);

			return Challenge(properties, provider);
		}
	}
}

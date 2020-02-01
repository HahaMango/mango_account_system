using MangoAccountSystem.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace MangoAccountSystem.Controllers
{
    public class AccountController : UserHelperController
    {
        private readonly SignInManager<MangoUser> _signInManager;
        private readonly UserManager<MangoUser> _userManager;
        private readonly RoleManager<MangoUserRole> _roleManager;

        public AccountController(SignInManager<MangoUser> signInManager,UserManager<MangoUser> userManager,RoleManager<MangoUserRole> roleManager)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        [HttpGet]
        public IActionResult Login(string returnUrl = null)
        {
            if(returnUrl == null)
            {
                returnUrl = "/center/home";
            }
            LoginViewModels loginViewModels = new LoginViewModels
            {
                ReturnUrl = returnUrl
            };
            return View(loginViewModels);
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginInputModels loginInputModels)
        {
            LoginViewModels loginView = new LoginViewModels();
            string username = loginInputModels.UserName;
            string password = loginInputModels.Password;

            await LoginUserNameValidation(loginView,username);
            await LoginPasswordValidation(loginView, password);

            if (loginView.IsError)
            {
                loginView.UserName = username;
                return View(loginView);
            }

            var success = await _signInManager.PasswordSignInAsync(username, password, false, false);
            if (!success.Succeeded)
            {
                loginView.ValidationErrors = "用户名和密码不正确";
                loginView.UserName = username;
                return View(loginView);
            }
            var mangouser = await _userManager.FindByNameAsync(username);
            mangouser.LastLoginDate = new DateTime();
            await _userManager.UpdateAsync(mangouser);

            return Redirect(loginInputModels.ReturnUrl);
        }

        [HttpGet]
        public IActionResult SignUp()
        {
            return View(new SignUpViewModel());
        }

        [HttpPost]
        public async Task<IActionResult> SignUp(SignUpInputModel signUpInputModel)
        {
            SignUpViewModel signUpViewModel = new SignUpViewModel();
            if (signUpInputModel.UserName == null || signUpInputModel.UserName == "")
            {
                signUpViewModel.UserNameErrors = "用户名不能为空";
            }
            if(signUpInputModel.Password == null || signUpInputModel.Password == "" || signUpInputModel.PasswordConfirm == null || signUpInputModel.PasswordConfirm == "")
            {
                signUpViewModel.PasswordErrors = "密码不能为空";
            }
            if(signUpInputModel.Password != signUpInputModel.PasswordConfirm)
            {
                signUpViewModel.PasswordErrors = "两次输入的密码不正确";
            }
            if (!signUpInputModel.IsAgree)
            {
                signUpViewModel.AgreeErrors = "请勾选同意协议";
            }
            if (signUpViewModel.IsAgreeError || signUpViewModel.IsPasswordError || signUpViewModel.IsUserNameError)
            {
                signUpViewModel.UserName = signUpInputModel.UserName;
                signUpViewModel.Email = signUpInputModel.Email;
                return View(signUpViewModel);
            }

            if (await _userManager.FindByNameAsync(signUpInputModel.UserName) != null)
            {
                signUpViewModel.UserNameErrors = "该用户名已存在";
                return View(signUpViewModel);
            }
            MangoUser mangoUser = new MangoUser
            {
                UserName = signUpInputModel.UserName,
                Email = signUpInputModel.Email
            };
            var flag = await _userManager.CreateAsync(mangoUser,signUpInputModel.Password);
            if (!flag.Succeeded)
            {
                ViewData["SignUpResult"] = "Registration error occurred!";
            }
            else
            {
                ViewData["SignUpResult"] = "Registration Successful!";
            }
            return View("ResultPage");
        }

        [HttpPost]
        public async Task<IActionResult> Logout(string userName, string returnUrl)
        {
            if(userName == null)
            {
                throw new ArgumentNullException();
            }

            string requestUrl = returnUrl;
            
            if(userName != User.Identity.Name && !User.Identity.IsAuthenticated)
            {
                return Redirect(requestUrl);
            }

            await _signInManager.SignOutAsync();

            return Redirect(requestUrl);
        }

        private async Task LoginUserNameValidation(LoginViewModels loginView,string username)
        {
            if(username == null || username == "")
            {
                loginView.ValidationErrors = "请填写用户名";
                return;
            }
            var mangouser = await _userManager.FindByNameAsync(username);
            if(mangouser == null)
            {
                loginView.ValidationErrors = "该用户名不存在";
                return;
            }
        }

        private async Task LoginPasswordValidation(LoginViewModels loginView,string password)
        {
            if(password == null || password == "")
            {
                loginView.ValidationErrors = "请填写密码";
                return;
            }
        }
    }
}
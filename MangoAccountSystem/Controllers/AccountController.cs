using MangoAccountSystem.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace MangoAccountSystem.Controllers
{
    public class AccountController : Controller
    {
        private SignInManager<MangoUser> _signInManager;
        private UserManager<MangoUser> _userManager;
        private RoleManager<MangoUserRole> _roleManager;

        public AccountController(SignInManager<MangoUser> signInManager,UserManager<MangoUser> userManager,RoleManager<MangoUserRole> roleManager)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public async Task<IActionResult> Login(string returnUrl = null)
        {
            var s = await _signInManager.PasswordSignInAsync("chiva_chen", "228887", false, false);

            return Redirect(returnUrl);
        }

        public async Task Test()
        {
            var mangoUser = await _userManager.FindByNameAsync("chiva_chen");

            var claims = await _signInManager.CreateUserPrincipalAsync(mangoUser);

            int i = 1;
        }
    }
}
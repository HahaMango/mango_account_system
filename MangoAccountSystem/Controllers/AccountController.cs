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
            string username = "haha";
            string password = "123";
            var claims = new List<Claim>
            {
                new Claim("username",username),
                new Claim("role","member"),
                new Claim("sub","1")
            };

            await HttpContext.SignInAsync(new ClaimsPrincipal(new ClaimsIdentity(claims, "Cookies", "username", "role")));

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
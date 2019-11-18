using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;

namespace MangoAccountSystem.Controllers
{
    public class AccountController : Controller
    {
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
    }
}
using MangoAccountSystem.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace MangoAccountSystem.Controllers
{
    public class ExternalController : UserHelperController
    {

        private readonly SignInManager<MangoUser> _signInManager;

        public ExternalController(SignInManager<MangoUser> signInManager)
        {
            _signInManager = signInManager;
        }

        public async Task<IActionResult> Callback(string returnUrl)
        {
            var info = await _signInManager.GetExternalLoginInfoAsync();
            return View();
        }
    }
}
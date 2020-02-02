using MangoAccountSystem.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Threading.Tasks;

namespace MangoAccountSystem.Controllers
{
    public class CenterController : UserHelperController
    {
        private readonly UserManager<MangoUser> _userManager;

        public CenterController(UserManager<MangoUser> userManager)
        {
            _userManager = userManager;
        }

        public IActionResult Home()
        {
            return View();
        }

        [Authorize]
        public async Task<IActionResult> User()
        {
            string username = base.User.Identity.Name;
            bool isauth = base.User.Identity.IsAuthenticated;
            if (!isauth)
            {
                return Redirect("/");
            }
            MangoUser mangoUser = await _userManager.FindByNameAsync(username);

            return View(mangoUser);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        [AllowAnonymous]
        public IActionResult Error()
        {
            var exception = HttpContext.Features.Get<IExceptionHandlerFeature>();
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier,Message = exception.Error.Message });
        }
    }
}

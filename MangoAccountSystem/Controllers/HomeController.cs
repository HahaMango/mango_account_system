using MangoAccountSystem.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace MangoAccountSystem.Controllers
{
    public class HomeController : UserHelperController
    {
        public IActionResult Index()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}

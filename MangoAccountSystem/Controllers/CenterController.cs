using MangoAccountSystem.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Threading.Tasks;

namespace MangoAccountSystem.Controllers
{
	/// <summary>
	/// 主页
	/// </summary>
    public class CenterController : UserHelperController
    {
		/// <summary>
		/// Identity用户管理器
		/// </summary>
        private readonly UserManager<MangoUser> _userManager;

		/// <summary>
		/// 授权方案服务
		/// </summary>
		private readonly IAuthenticationSchemeProvider _authenticationSchemeProvider;

        public CenterController(UserManager<MangoUser> userManager, IAuthenticationSchemeProvider authenticationSchemeProvider)
        {
            _userManager = userManager;
            _authenticationSchemeProvider = authenticationSchemeProvider;
        }

		/// <summary>
		/// 主页入口
		/// </summary>
		/// <returns></returns>
        public IActionResult Home()
        {
            return View();
        }

		/// <summary>
		/// 用户信息入口
		/// </summary>
		/// <returns></returns>
        [Authorize]
        public async Task<IActionResult> User()
        {
            bool isauth = base.User.Identity.IsAuthenticated;
            if (!isauth)
            {
                return Redirect("/");
            }
            string username = base.User.Identity.Name;

            MangoUser mangoUser = await _userManager.FindByNameAsync(username);

            return View(mangoUser);
        }

		/// <summary>
		/// 错误页面
		/// </summary>
		/// <returns></returns>
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        [AllowAnonymous]
        public IActionResult Error()
        {
            var exception = HttpContext.Features.Get<IExceptionHandlerFeature>();
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier,Message = exception.Error.Message });
        }
    }
}

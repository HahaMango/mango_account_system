using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Filters;

namespace MangoAccountSystem.Controllers
{
	/// <summary>
	/// 拦截请求，在Action之前执行，读取授权信息。
	/// </summary>
    public class UserHelperController : Microsoft.AspNetCore.Mvc.Controller
    {
        public override async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            await Task.Run(() =>
            {
                ViewData["IsAuthenticated"] = User.Identity.IsAuthenticated;
                ViewData["UserName"] = User.Identity.Name;
            });
            await base.OnActionExecutionAsync(context, next);
        }
    }
}

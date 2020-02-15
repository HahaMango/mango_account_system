using IdentityServer4.Services;
using MangoAccountSystem.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace MangoAccountSystem.Controllers
{
    public class ExternalController : UserHelperController
    {
        private readonly IIdentityServerInteractionService _interaction;
        private readonly SignInManager<MangoUser> _signInManager;

        public ExternalController(SignInManager<MangoUser> signInManager, IIdentityServerInteractionService identityServerInteractionService)
        {
            _signInManager = signInManager;
            _interaction = identityServerInteractionService;
        }

        [HttpGet]
        public async Task<IActionResult> Callback(string returnUrl)
        {
            var info = await _signInManager.GetExternalLoginInfoAsync();
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> Challenge(string returnUrl,string provider)
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

            return Challenge(properties,provider);
        }
    }
}
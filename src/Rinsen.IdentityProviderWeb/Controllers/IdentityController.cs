using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using Rinsen.IdentityProvider.Core;
using Rinsen.IdentityProviderWeb.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Rinsen.IdentityProviderWeb.Controllers
{
    public class IdentityController : Controller
    {
        private readonly ILoginService _loginService;

        [HttpGet]
        [AllowAnonymous]
        public IActionResult Login(string returnUrl)
        {

            

            return View(new LoginModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginModel model)
        {
            if (ModelState.IsValid)
            {
                var result = await _loginService.LoginAsync(model.Email, model.Password, model.RememberMe);

                if (result.Succeeded)
                {
                    return RedirectToLocalOrTrustedHostOnly(model.ReturnUrl);
                }

                ModelState.AddModelError(string.Empty, "Invalid login attempt.");
            }

            return View(model);
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult Create()
        {



            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(LoginModel model)
        {


            return View(model);
        }

        [HttpGet]
        public IActionResult Logout()
        {



            return View();
        }

        private IActionResult RedirectToLocalOrTrustedHostOnly(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }

            var result = _externalHostValidator.ValidateAsync(returnUrl);

            if (result.Succeeded)
            {
                // This
                var uriBuilder = new UriBuilder(returnUrl);
                var query = QueryHelpers.ParseQuery(uriBuilder.Query);
                query["Token"] = result.Token;
                uriBuilder.Query = query.ToString();

                // Or this?
                var uri = QueryHelpers.AddQueryString(returnUrl, "Token", result.Token);

                return Redirect(uriBuilder.ToString());
            }

            return RedirectToAction(nameof(HomeController.Index), "Home");
        }
    }
}

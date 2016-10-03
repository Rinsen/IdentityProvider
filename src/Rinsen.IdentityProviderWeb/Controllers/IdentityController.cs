using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using Rinsen.IdentityProvider.Core;
using Rinsen.IdentityProvider.Core.ExternalApplications;
using Rinsen.IdentityProvider.Core.LocalAccounts;
using Rinsen.IdentityProviderWeb.Models;
using System;
using System.Threading.Tasks;

namespace Rinsen.IdentityProviderWeb.Controllers
{
    public class IdentityController : Controller
    {
        private readonly ILoginService _loginService;
        private readonly IExternalApplicationService _externalApplicationService;
        private readonly IIdentityService _identityService;
        private readonly ILocalAccountService _localAccountService;

        public IdentityController(ILoginService loginService,
            IExternalApplicationService externalApplicationService,
            IIdentityService identityService,
            ILocalAccountService localAccountService)
        {
            _loginService = loginService;
            _externalApplicationService = externalApplicationService;
            _identityService = identityService;
            _localAccountService = localAccountService;
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult Login(string returnUrl)
        {

            

            return View(new LoginModel { ReturnUrl = returnUrl } );
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
                    return await RedirectToLocalOrTrustedHostOnlyAsync(model.ReturnUrl, result.Identity.IdentityId);
                }

                ModelState.AddModelError(string.Empty, "Invalid login attempt.");
            }

            return View(model);
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult Create()
        {
            


            return View(new CreateIdentityModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateIdentityModel model)
        {
            if (ModelState.IsValid)
            {
                var createIdentityResult = await _identityService.CreateAsync(model.GivenName, model.Surname, model.Email, model.PhoneNumber);

                if (createIdentityResult.Succeeded)
                {
                    var createLocalAccountResult = await _localAccountService.CreateAsync(createIdentityResult.Identity.IdentityId, model.Email, model.Password);

                    if (createLocalAccountResult.Succeeded)
                    {
                        var loginResult = await _loginService.LoginAsync(model.Email, model.Password, false);

                        if (loginResult.Succeeded)
                        {
                            return await RedirectToLocalOrTrustedHostOnlyAsync(model.ReturnUrl, createIdentityResult.Identity.IdentityId);
                        }
                    }
                }

                ModelState.AddModelError(string.Empty, "Create user invalid.");
            }

            return View(model);
        }

        [HttpGet]
        public IActionResult Logout()
        {



            return View();
        }

        private async Task<IActionResult> RedirectToLocalOrTrustedHostOnlyAsync(string returnUrl, Guid identityId)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }

            var result = await _externalApplicationService.GetTokenForValidHostAsync(returnUrl, identityId);

            if (result.Succeeded)
            {
                var uri = QueryHelpers.AddQueryString(returnUrl, "Token", result.Token);

                return Redirect(uri);
            }

            return RedirectToAction(nameof(HomeController.Index), "Home");
        }
    }
}

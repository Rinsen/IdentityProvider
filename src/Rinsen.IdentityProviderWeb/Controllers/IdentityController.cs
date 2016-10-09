using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using Rinsen.IdentityProvider.Core;
using Rinsen.IdentityProvider.Core.ExternalApplications;
using Rinsen.IdentityProvider.Core.LocalAccounts;
using Rinsen.IdentityProviderWeb.Models;
using System;
using System.Collections.Generic;
using System.Security.Claims;
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
        public async Task<IActionResult> Login(string returnUrl, string host)
        {
            if (User.Identity.IsAuthenticated)
            {
                var identityId = User.GetClaimGuidValue(ClaimTypes.NameIdentifier);

                return await RedirectToLocalOrTrustedHostOnlyAsync(returnUrl, host, identityId);
            }

            return View(new LoginModel { ReturnUrl = returnUrl, Host = host } );
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
                    return await RedirectToLocalOrTrustedHostOnlyAsync(model.ReturnUrl, model.Host, result.Identity.IdentityId);
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
                            return await RedirectToLocalOrTrustedHostOnlyAsync(model.ReturnUrl, model.Host, createIdentityResult.Identity.IdentityId);
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

        private async Task<IActionResult> RedirectToLocalOrTrustedHostOnlyAsync(string returnUrl, string host, Guid identityId)
        {
            if (!string.IsNullOrEmpty(host))
            {
                var result = await _externalApplicationService.GetTokenForValidHostAsync(host, identityId);

                if (result.Succeeded)
                {

                    var uri = $"https://{host}{returnUrl}" + QueryString.Create("AuthToken", result.Token).ToUriComponent();

                    return Redirect(uri);
                }
            }

            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }

            return RedirectToAction(nameof(HomeController.Index), nameof(HomeController).Substring(0, nameof(HomeController).Length - 10));
        }
    }
}

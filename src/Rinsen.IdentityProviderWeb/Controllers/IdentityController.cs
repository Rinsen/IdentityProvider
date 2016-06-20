using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Rinsen.IdentityProvider.Core;
using Rinsen.IdentityProvider.Core.LocalAccounts;
using Rinsen.IdentityProvider.Core.Sessions;
using Rinsen.IdentityProviderWeb.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace Rinsen.IdentityProviderWeb.Controllers
{
    public class IdentityController : Controller
    {
        readonly IIdentityService _identityService;
        readonly ISessionHandler _sessionHandler;
        readonly IdentityOptions _identityOptions;
        readonly ILocalAccountService _localAccountService;
        readonly ILogger<IdentityController> _log;
        readonly IIdentityAccessor _identityAccessor;

        public IdentityController(IIdentityService identityService,
            ISessionHandler sessionHandler,
            IdentityOptions identityOptions,
            IIdentityAccessor identityAccessor,
            ILocalAccountService localAccountService,
            ILogger<IdentityController> log)
        {
            _identityService = identityService;
            _sessionHandler = sessionHandler;
            _identityOptions = identityOptions;
            _identityAccessor = identityAccessor;
            _localAccountService = localAccountService;
            _log = log;
        }

        [AllowAnonymous]
        [HttpGet]
        public IActionResult Index(string returnUrl)
        {
            var identityModel = new IdentityModel();

            if (!User.Identity.IsAuthenticated)
            {
                identityModel.LoginModel = new LoginModel();
                identityModel.CreateIdentityModel = new CreateIdentityModel();

                if (string.IsNullOrEmpty(returnUrl) && Request.Headers.ContainsKey("Referer"))
                    returnUrl = WebUtility.UrlEncode(Request.Headers["Referer"]);

                if (/*Url.IsLocalUrl(returnUrl) && */!string.IsNullOrEmpty(returnUrl))
                {
                    identityModel.LoginModel.ReturnUrl = returnUrl;
                }
            }
            else
            {
                identityModel.IdentityDetailsModel = CreateIdentityDetailsModel();
                identityModel.ChangePasswordModel = new ChangePasswordModel();
            }

            return View(identityModel);
        }

        IdentityDetailsModel CreateIdentityDetailsModel()
        {
            var identity = _identityService.GetIdentity();

            return new IdentityDetailsModel
            {
                FirstName = identity.FirstName,
                LastName = identity.LastName,
                PhoneNumber = identity.PhoneNumber,
                Email = identity.Email,
            };
        }

        [AllowAnonymous]
        [HttpPost]
        public ActionResult Create(IdentityModel model)
        {
            model.LoginModel = new LoginModel();
            try
            {
                if (ModelState.IsValid)
                {
                    var identity = model.CreateIdentityModel.MapToIdentity();

                    _identityService.CreateIdentity(identity);

                    _localAccountService.CreateLocalAccount(identity.IdentityId, model.CreateIdentityModel.Email, model.CreateIdentityModel.Password);
                    
                    _log.LogInformation("New identity created for email {0}, with name {1}, {2} and phone number {3}", identity.Email, identity.FirstName, identity.LastName, identity.PhoneNumber);

                    var sessionId = _sessionHandler.CreateSession(model.CreateIdentityModel.Email, model.CreateIdentityModel.Password);

                    CreateSessionCookie(sessionId, false);

                    return RedirectToAction("Index", "Home");
                }
            }
            catch (IdentityAlreadyExistException ex)
            {
                _log.LogWarning("Identity already exist for email {0}, with name {1}, {2} and phone number {3}",
                    model.CreateIdentityModel.Email, model.CreateIdentityModel.FirstName, model.CreateIdentityModel.LastName, model.CreateIdentityModel.PhoneNumber);

                ModelState.AddModelError(nameof(CreateIdentityModel) + ".Email", ex.Message);
            }

            return View("Index", model);
        }

        [AllowAnonymous]
        [HttpPost]
        public ActionResult Login(IdentityModel loginAndCreateModel)
        {
            var returnUrl = loginAndCreateModel.LoginModel.GetDecodedReturnUrl();

            var sessionId = _sessionHandler.CreateSession(loginAndCreateModel.LoginModel.Email, loginAndCreateModel.LoginModel.Password);

            if (!string.IsNullOrEmpty(sessionId))
            {
                CreateSessionCookie(sessionId, loginAndCreateModel.LoginModel.RememberMe);

                if (Url.IsLocalUrl(returnUrl))
                {
                    return Redirect(returnUrl);
                }

                return RedirectToAction("Index", "Home");
            }

            loginAndCreateModel.LoginModel.InvalidEmailOrPassword = true;
            loginAndCreateModel.LoginModel.Password = string.Empty;

            return View("Index", loginAndCreateModel);
        }

        void CreateSessionCookie(string sessionId, bool remeberMe)
        {
            if (remeberMe)
            {
                Response.Cookies.Append(_identityOptions.SessionKeyName, sessionId, new CookieOptions { Expires = DateTime.Now.AddYears(10), HttpOnly = true, Secure = _identityOptions.SessionCookieOnlySecureTransfer });
            }
            else
            {
                Response.Cookies.Append(_identityOptions.SessionKeyName, sessionId, new CookieOptions { HttpOnly = true, Secure = _identityOptions.SessionCookieOnlySecureTransfer });
            }

            Response.Headers.Add("Cache-Control", "no-cache");
            Response.Headers.Add("Pragma", "no-cache");
            Response.Headers.Add("Expires", "-1");
        }

        [HttpGet]
        public ActionResult Logout()
        {
            _sessionHandler.DeleteSession();

            Response.Cookies.Append(_identityOptions.SessionKeyName, "", new CookieOptions { Expires = DateTime.Now.AddDays(-2) });

            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        public ActionResult ChangePassword(IdentityModel model)
        {
            try
            {
                _localAccountService.ChangePassword(model.ChangePasswordModel.CurrentPassword, model.ChangePasswordModel.Password);
                model.ChangePasswordModel.SetValidPasswordState();
            }
            catch (UnauthorizedAccessException e)
            {
                _log.LogWarning(null, e, "Invalid password for identity {0}", _identityAccessor.IdentityId);
                ModelState.AddModelErrorAndClearModelValue<IdentityModel>(m => m.ChangePasswordModel.CurrentPassword, "Password is not correct");
                model.ChangePasswordModel.SetInvalidCurrentPasswordState();
            }

            ModelState.ClearModelValue<IdentityModel>(m => m.ChangePasswordModel.CurrentPassword);
            ModelState.ClearModelValue<IdentityModel>(m => m.ChangePasswordModel.Password);
            ModelState.ClearModelValue<IdentityModel>(m => m.ChangePasswordModel.PasswordRepeated);

            model.IdentityDetailsModel = CreateIdentityDetailsModel();
            return View("Index", model);
        }

        [HttpPost]
        public ActionResult UpdateIdentityDetails(IdentityModel model)
        {
            try
            {
                _localAccountService.ValidatePassword(model.IdentityDetailsModel.Password);
                _identityService.UpdateIdentityDetails(model.IdentityDetailsModel.FirstName, model.IdentityDetailsModel.LastName, model.IdentityDetailsModel.Email, model.IdentityDetailsModel.PhoneNumber);
            }
            catch (Exception e)
            {
                _log.LogWarning(null, e, "Invalid password for identity {0}", _identityAccessor.IdentityId);
                ModelState.AddModelErrorAndClearModelValue<IdentityModel>(m => m.IdentityDetailsModel.Password, "Password is not correct");
                model.ChangePasswordModel.SetInvalidCurrentPasswordState();
            }

            model.ChangePasswordModel = new ChangePasswordModel();

            return View("Index", model);
        }
    }
}

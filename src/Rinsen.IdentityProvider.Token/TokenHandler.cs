using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http.Features.Authentication;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Security.Claims;
using Microsoft.AspNetCore.Http.Authentication;
using Microsoft.Extensions.Logging;
using Rinsen.IdentityProvider.Core.ExternalApplications.v1;
using Microsoft.AspNetCore.Http;

namespace Rinsen.IdentityProvider.Token
{
    public class TokenHandler : AuthenticationHandler<TokenOptions>
    {
        private readonly ILogger<TokenHandler> _log;

        public TokenHandler(ILogger<TokenHandler> log)
        {
            _log = log;
        }

        protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            // Only run this if user is not authenticated
            if (Context.User.Identity.IsAuthenticated)
            {
                return AuthenticateResult.Skip();
            }

            string authorizationToken = Request.Query["AuthToken"];

            if (string.IsNullOrEmpty(authorizationToken))
            {
                return AuthenticateResult.Fail("No authorization header.");
            }

            if (string.IsNullOrEmpty(Options.ApplicationKey))
            {
                throw new InvalidOperationException("No application key is provided");
            }
            try
            {
                ExternalIdentityResult externalIdentity;
                using (var httpClient = new HttpClient())
                {
                    var validationUrl = Options.ValidateTokenPath + 
                        QueryString.Create(new[] 
                        {
                            new KeyValuePair<string, string>(Options.TokenParameterName, authorizationToken),
                            new KeyValuePair<string, string>(Options.ApplicationKeyParameterName, Options.ApplicationKey)
                        }).ToUriComponent();
                    using (var stream = await httpClient.GetStreamAsync(validationUrl))
                    using (StreamReader streamReader = new StreamReader(stream))
                    using (JsonReader reader = new JsonTextReader(streamReader))
                    {
                        JsonSerializer serializer = new JsonSerializer();

                        externalIdentity = serializer.Deserialize<ExternalIdentityResult>(reader);
                    }

                    var claims = new List<Claim>
                            {
                                new Claim(ClaimTypes.NameIdentifier, externalIdentity.IdentityId.ToString(), nameof(Guid), externalIdentity.Issuer),
                                new Claim(ClaimTypes.Name, $"{externalIdentity.GivenName} {externalIdentity.Surname}", externalIdentity.Issuer),
                                new Claim(ClaimTypes.Email, externalIdentity.Email, externalIdentity.Issuer),
                                new Claim(ClaimTypes.MobilePhone, externalIdentity.PhoneNumber, externalIdentity.Issuer),
                                new Claim(ClaimTypes.GivenName, externalIdentity.GivenName, externalIdentity.Issuer),
                                new Claim(ClaimTypes.Surname, externalIdentity.Surname, externalIdentity.Issuer)
                            };

                    var claimsIdentiy = new ClaimsIdentity(claims, Options.AuthenticationScheme);

                    var claimsProvider = new ClaimsPrincipal(claimsIdentiy);

                    await Context.Authentication.SignInAsync(Options.CookieAuthenticationScheme, claimsProvider, new AuthenticationProperties());

                    return AuthenticateResult.Success(new AuthenticationTicket(claimsProvider, new AuthenticationProperties(), Options.AuthenticationScheme));
                }
            }
            catch (Exception e)
            {
                _log.LogError(1, e, $"Validate token {authorizationToken} failed");
                return AuthenticateResult.Fail(e);
            }
        }

        protected override Task HandleSignOutAsync(SignOutContext context)
        {
            throw new NotSupportedException();
        }

        protected override Task HandleSignInAsync(SignInContext context)
        {
            throw new NotSupportedException();
        }

        protected override Task<bool> HandleUnauthorizedAsync(ChallengeContext context)
        {
            if (Request.Query.ContainsKey("AuthToken"))
            {
                throw new InvalidOperationException("Possible auth loop detected");
            }

            var redirectUrl = OriginalPathBase + Request.Path + Request.QueryString;

            var loginUri = Options.LoginPath + QueryString.Create(new[]
                        {
                            new KeyValuePair<string, string>(Options.ReturnUrlParamterName, redirectUrl),
                            new KeyValuePair<string, string>(Options.HostParameterName, Request.Host.Value)
                        }).ToUriComponent();

            if (IsAjaxRequest(Request))
            {
                Response.Headers["Location"] = loginUri;
                Response.StatusCode = 401;
            }
            else
            {
                Response.Redirect(loginUri);
            }

            return Task.FromResult(true);
        }

        private static bool IsAjaxRequest(HttpRequest request)
        {
            return string.Equals(request.Query["X-Requested-With"], "XMLHttpRequest", StringComparison.Ordinal) ||
                string.Equals(request.Headers["X-Requested-With"], "XMLHttpRequest", StringComparison.Ordinal);
        }
    }
}

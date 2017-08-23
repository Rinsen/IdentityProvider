using Microsoft.AspNetCore.Authentication;
using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Text.Encodings.Web;
using Rinsen.IdentityProvider.Contracts.v1;
using System.Net.Http;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using System.Security.Claims;
using System.Linq;
using Rinsen.IdentityProvider.Contracts;

namespace Rinsen.IdentityProvider.Token
{
    public class RemoteTokenHandler : RemoteAuthenticationHandler<TokenOptions>
    {
        public RemoteTokenHandler(IOptionsMonitor<TokenOptions> options, ILoggerFactory logger, UrlEncoder encoder, ISystemClock clock) 
            : base(options, logger, encoder, clock)
        {
        }

        protected override async Task<HandleRequestResult> HandleRemoteAuthenticateAsync()
        {
            // Only run this if user is not authenticated
            if (Context.User.Identity.IsAuthenticated)
            {
                return HandleRequestResult.SkipHandler();
            }

            string authorizationToken = Request.Query["AuthToken"];

            if (string.IsNullOrEmpty(authorizationToken))
            {
                return HandleRequestResult.Fail("No authorization header.");
            }

            if (string.IsNullOrEmpty(Options.ApplicationKey))
            {
                throw new InvalidOperationException("No application key is provided");
            }
            try
            {
                ExternalIdentity externalIdentity;
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

                        externalIdentity = serializer.Deserialize<ExternalIdentity>(reader);
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

                    if (externalIdentity.Extensions.Any(c => c.Type == RinsenIdentityConstants.Role && c.Value == RinsenIdentityConstants.Administrator))
                    {
                        claims.Add(new Claim(ClaimTypes.Role, RinsenIdentityConstants.Administrator, externalIdentity.Issuer));
                    }

                    var claimsIdentiy = new ClaimsIdentity(claims, Options.ClaimsIssuer);

                    var claimsPrincipal = new ClaimsPrincipal(claimsIdentiy);

                    //await Context.SignInAsync(Options.AuthenticationScheme, claimsPrincipal);

                    //await Options.Events.AuthenticationSucceeded(new AuthenticationSucceededContext { ClaimsPrincipal = claimsPrincipal });

                    return HandleRequestResult.Success(new AuthenticationTicket(claimsPrincipal, TokenDefaults.AuthenticationScheme));
                }
            }
            catch (Exception e)
            {
                Logger.LogError(1, e, $"Validate token {authorizationToken} failed");
                return HandleRequestResult.Fail(e);
            }
        }

        public override Task<bool> ShouldHandleRequestAsync()
        {
            return base.ShouldHandleRequestAsync();
        }

        protected override Task HandleChallengeAsync(AuthenticationProperties properties)
        {
            if (Request.Query.ContainsKey("AuthToken"))
            {
                throw new InvalidOperationException("Possible auth loop detected");
            }

            var redirectUrl = "/token";// OriginalPathBase + Request.Path + Request.QueryString;

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

            return Task.CompletedTask;
        }

        private static bool IsAjaxRequest(HttpRequest request)
        {
            return string.Equals(request.Query["X-Requested-With"], "XMLHttpRequest", StringComparison.Ordinal) ||
                string.Equals(request.Headers["X-Requested-With"], "XMLHttpRequest", StringComparison.Ordinal);
        }
    }
}

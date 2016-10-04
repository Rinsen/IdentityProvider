using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http.Features.Authentication;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Rinsen.IdentityProvider.Core.ExternalApplications;
using System.Security.Claims;
using Microsoft.AspNetCore.Http.Authentication;

namespace Rinsen.IdentityProvider.Token
{
    public class TokenHandler : AuthenticationHandler<TokenOptions>
    {
        

        protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            string authorizationToken = Request.Query["AuthToken"];

            if (string.IsNullOrEmpty(authorizationToken))
            {
                return AuthenticateResult.Fail("No authorization header.");
            }

            ExternalIdentity externalIdentity;
            using (var httpClient = new HttpClient())
            {
                using (var stream = await httpClient.GetStreamAsync($"{Options.ExternalIdentityProviderBaseAddress}/api/Identity/Get?Token={authorizationToken}&ApplicationKey={Options.ApplicationKey}"))
                using (StreamReader streamReader = new StreamReader(stream))
                using (JsonReader reader = new JsonTextReader(streamReader))
                {
                    JsonSerializer serializer = new JsonSerializer();

                    externalIdentity = serializer.Deserialize<ExternalIdentity>(reader);
                }

                var claims = new List<Claim>
                            {
                                new Claim(ClaimTypes.NameIdentifier, externalIdentity.IdentityId.ToString(), nameof(Guid), externalIdentity.Issuer),
                                new Claim(ClaimTypes.Name, $"{externalIdentity.GivenName} {externalIdentity.Surname}")
                            };

                var claimsIdentiy = new ClaimsIdentity(claims, Options.AuthenticationScheme);

                var claimsProvider = new ClaimsPrincipal(claimsIdentiy);

                await Context.Authentication.SignInAsync(Options.CookieAuthenticationScheme, claimsProvider);

                return AuthenticateResult.Success(new AuthenticationTicket(claimsProvider, new AuthenticationProperties(), Options.AuthenticationScheme));
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
    }
}

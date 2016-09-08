using Microsoft.AspNetCore.Authentication;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http.Features.Authentication;
using System.Security.Claims;
using Microsoft.AspNetCore.Http.Authentication;
using System.Net.Http;
using Rinsen.IdentityProvider.Core.ExternalApplications;
using Newtonsoft.Json;
using System.IO;

namespace Rinsen.IdentityProvider.Client
{
    public class RinsenAuthenticationHandler : AuthenticationHandler<RinsenAuthenticationOptions>
    {

        protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            if (Context.Request.Query.ContainsKey(Options.TokenKey))
            {
                var identity = await GetIdentityFromTokenAsync(Context.Request.Query[Options.TokenKey]);
            }

            var claims = new[]
                            {
                                new Claim(ClaimTypes.NameIdentifier, "hejsan"),
                                new Claim(ClaimTypes.Name, "hoppsan")
                            };



            var authTicket = new AuthenticationTicket(
                new ClaimsPrincipal(new ClaimsIdentity(claims, RinsenAuthenticationDefaults.AuthenticationScheme)),
                new AuthenticationProperties(),
                RinsenAuthenticationDefaults.AuthenticationScheme);

            return AuthenticateResult.Success(authTicket);
        }

        protected async Task<ExternalIdentity> GetIdentityFromTokenAsync(string token)
        {
            using (var httpClient = new HttpClient())
            {
                // Add application key

                using (var stream = await httpClient.GetStreamAsync(string.Format("{0}/api/Identity/Get?Token={1}", Options.IdentityProviderBaseAddress)))
                using (StreamReader streamReader = new StreamReader(stream))
                using (JsonReader reader = new JsonTextReader(streamReader))
                {
                    JsonSerializer serializer = new JsonSerializer();
                    
                    return serializer.Deserialize<ExternalIdentity>(reader);
                }
            }
        }
    }
}

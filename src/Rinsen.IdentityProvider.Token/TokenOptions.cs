using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Options;
using Rinsen.IdentityProvider.Core;
using System;

namespace Rinsen.IdentityProvider.Token
{
    public class TokenOptions : RemoteAuthenticationOptions, IOptions<TokenOptions>
    {
        public TokenOptions()
        {
            Events = new RemoteAuthenticationEvents
            {
                OnTicketReceived = async context => 
                {
                    if (string.IsNullOrEmpty(ConnectionString))
                    {
                        throw new ArgumentException($"No {nameof(ConnectionString)} provided in {nameof(TokenOptions)}");
                    }

                    var localIdentityForReferenceHandler = new LocalIdentityForReferenceHandler(ConnectionString);
                    await localIdentityForReferenceHandler.CreateReferenceIdentityIfNotExists(context.Principal);
                }
            };
        }
        
        public string ApplicationKey { get; set; }
        public string ValidateTokenPath { get; set; }
        public string LoginPath { get; set; }
        public string ConnectionString { get; set; }

        public TokenOptions Value { get { return this; } }

        public string AuthenticationScheme { get; set; } = "Cookies";
        public string ReturnUrlParamterName { get; set; } = "ReturnUrl";
        public string ApplicationKeyParameterName { get; set; } = "ApplicationKey";
        public string TokenParameterName { get; set; } = "AuthToken";
        public string HostParameterName { get; set; } = "Host";

    }
}

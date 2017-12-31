using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Options;
using Rinsen.IdentityProvider.Core;
using System;

namespace Rinsen.IdentityProvider.Token
{
    public class TokenOptions : RemoteAuthenticationOptions, IOptions<TokenOptions>
    {
        public string ApplicationKey { get; set; }
        public string IdentityServiceUrl { get { return _identityServiceUrl; } set { _identityServiceUrl = value.TrimEnd('/'); } }
        private string _identityServiceUrl;

        public TokenOptions Value { get { return this; } }

        public string AuthenticationScheme { get; set; } = "Cookies";
        public string ReturnUrlParamterName { get; set; } = "ReturnUrl";
        public string ApplicationKeyParameterName { get; set; } = "ApplicationKey";
        public string TokenParameterName { get; set; } = "AuthToken";
        public string HostParameterName { get; set; } = "Host";

    }
}

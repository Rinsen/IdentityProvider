using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Options;

namespace Rinsen.IdentityProvider.Token
{
    public class TokenOptions : AuthenticationOptions, IOptions<TokenOptions>
    {
        public TokenOptions()
        {
            AutomaticAuthenticate = true;
            AutomaticChallenge = true;
            AuthenticationScheme = TokenDefaults.AuthenticationScheme;
            CookieAuthenticationScheme = "RinsenCookie";
            ReturnUrlParamterName = "ReturnUrl";
            ApplicationKeyParameterName = "ApplicationKey";
            HostParameterName = "Host";
            TokenParameterName = "AuthToken";
        }

        public string ApplicationKey { get; set; }
        public string ValidateTokenPath { get; set; }
        public string LoginPath { get; set; }
        public string CookieAuthenticationScheme { get; set; }
        public TokenOptions Value { get { return this; } }
        public string ReturnUrlParamterName { get; set; }
        public string ApplicationKeyParameterName { get; set; }
        public string TokenParameterName { get; set; }
        public string HostParameterName { get; set; }
        public ITokenAuthenticationEvents Events { get; set; } = new TokenAuthenticationEvents();

    }
}

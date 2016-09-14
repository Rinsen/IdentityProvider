using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Options;

namespace Rinsen.IdentityProvider.Token
{
    public class TokenOptions : AuthenticationOptions, IOptions<TokenOptions>
    {
        public TokenOptions()
        {
            AuthenticationScheme = TokenDefaults.AuthenticationScheme;
            CookieAuthenticationScheme = "RinsenCookie";
        }

        public string ApplicationKey { get; set; }

        public string ExternalIdentityProviderBaseAddress { get; set; }

        public string CookieAuthenticationScheme { get; set; }

        public TokenOptions Value { get { return this; } }

    }
}

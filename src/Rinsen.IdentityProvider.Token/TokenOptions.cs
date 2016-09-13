using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Options;

namespace Rinsen.IdentityProvider.Token
{
    public class TokenOptions : AuthenticationOptions, IOptions<TokenOptions>
    {
        public string ExternalIdentityProviderIdentifier { get; set; }

        public string ExternalIdentityProviderBaseAddress { get; set; }

        public string CookieAuthenticationScheme { get; set; }

        public TokenOptions Value { get { return this; } }
    }
}

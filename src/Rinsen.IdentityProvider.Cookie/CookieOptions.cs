using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Rinsen.IdentityProvider.Cookie
{
    public class CookieOptions : AuthenticationOptions, IOptions<CookieOptions>
    {

        public CookieOptions Value { get { return this; } }
    }
}

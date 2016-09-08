using Microsoft.AspNetCore.Authentication;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Text.Encodings.Web;

namespace Rinsen.IdentityProvider.Token
{
    public class TokenMiddleware : AuthenticationMiddleware<TokenOptions>
    {
        public TokenMiddleware(RequestDelegate next, IOptions<TokenOptions> options, ILoggerFactory loggerFactory, UrlEncoder encoder)
            : base(next, options, loggerFactory, encoder)
        {
        }

        protected override AuthenticationHandler<TokenOptions> CreateHandler()
        {
            throw new NotImplementedException();
        }
    }
}

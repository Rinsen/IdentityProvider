using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Text.Encodings.Web;

namespace Rinsen.IdentityProvider.Cookie
{
    public class CookieMiddleware : AuthenticationMiddleware<CookieOptions>
    {
        public CookieMiddleware(RequestDelegate next,
            IOptions<CookieOptions> options,
            ILoggerFactory loggerFactory,
            UrlEncoder encoder)
            : base(next, options, loggerFactory, encoder)
        {
        }

        protected override AuthenticationHandler<CookieOptions> CreateHandler()
        {
            return new CookieHandler();
        }
    }
}

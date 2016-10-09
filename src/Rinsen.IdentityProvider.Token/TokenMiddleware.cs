using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Text.Encodings.Web;

namespace Rinsen.IdentityProvider.Token
{
    public class TokenMiddleware : AuthenticationMiddleware<TokenOptions>
    {
        private readonly ILoggerFactory _loggerFactory;

        public TokenMiddleware(RequestDelegate next, IOptions<TokenOptions> options, ILoggerFactory loggerFactory, UrlEncoder encoder)
            : base(next, options, loggerFactory, encoder)
        {
            _loggerFactory = loggerFactory;
        }

        protected override AuthenticationHandler<TokenOptions> CreateHandler()
        {
            return new TokenHandler(_loggerFactory.CreateLogger<TokenHandler>());
        }
    }
}

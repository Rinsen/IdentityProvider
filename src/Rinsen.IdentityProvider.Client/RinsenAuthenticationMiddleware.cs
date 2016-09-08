using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Rinsen.IdentityProvider.Client
{
    public class RinsenAuthenticationMiddleware : AuthenticationMiddleware<RinsenAuthenticationOptions>
    {
        public RinsenAuthenticationMiddleware(
            RequestDelegate next,
            IOptions<RinsenAuthenticationOptions> options,
            ILoggerFactory loggerFactory,
            UrlEncoder encoder)
            : base(next, options, loggerFactory, encoder)
        {
            
        }

        protected override AuthenticationHandler<RinsenAuthenticationOptions> CreateHandler()
        {
            return new RinsenAuthenticationHandler();
        }
    }
}

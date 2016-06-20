using System;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace Rinsen.IdentityProvider.Core
{
    public class AuthenticationMiddleware
    {
        readonly RequestDelegate _next;
        readonly IdentityProvider _identityProvider;
        readonly IdentityOptions _options;

        public AuthenticationMiddleware(RequestDelegate next, IdentityProvider identityProvider, IdentityOptions options)
        {
            _next = next;
            _identityProvider = identityProvider;
            _options = options;
        }

        public async Task Invoke(HttpContext context)
        {
            if (context.Request.Cookies.ContainsKey(_options.SessionKeyName))
            {
                // Authenticate from session identifier
                AuthenticateSessionIdentifier(context);
            }

            await _next(context);
        }

        void AuthenticateSessionIdentifier(HttpContext context)
        {
            var sessionId = context.Request.Cookies[_options.SessionKeyName];

            ClaimsPrincipal claimsPrincipal;
            if (_identityProvider.TryGetClaimsPrincipalFromSessionId(sessionId, out claimsPrincipal))
            {
                context.User = claimsPrincipal;
            }
            else
            {
                context.Response.Cookies.Append(_options.SessionKeyName, "", new CookieOptions { Expires = DateTime.Now.AddDays(-2), HttpOnly = true, Secure = _options.SessionCookieOnlySecureTransfer });
            }
        }
    }
}

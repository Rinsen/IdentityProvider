using Microsoft.AspNetCore.Builder;
using System;

namespace Rinsen.IdentityProvider.Token
{
    public static class TokenAppBuilderExtensions
    {
        public static IApplicationBuilder UseTokenAuthentication(this IApplicationBuilder app)
        {
            if (app == null)
            {
                throw new ArgumentNullException(nameof(app));
            }

            return app.UseMiddleware<TokenMiddleware>();
        }

        public static IApplicationBuilder UseTokenAuthentication(this IApplicationBuilder app, TokenOptions options)
        {
            if (app == null)
            {
                throw new ArgumentNullException(nameof(app));
            }

            if (options == null)
            {
                throw new ArgumentNullException(nameof(options));
            }

            return app.UseMiddleware<TokenMiddleware>(options);
        }

        public static IApplicationBuilder UseTokenAuthenticationWithCookieAuthentication(this IApplicationBuilder app, TokenOptions options, CookieAuthenticationOptions cookieOptions)
        {
            if (app == null)
            {
                throw new ArgumentNullException(nameof(app));
            }

            if (options == null)
            {
                throw new ArgumentNullException(nameof(options));
            }

            if (cookieOptions == null)
            {
                throw new ArgumentNullException(nameof(cookieOptions));
            }

            app.UseCookieAuthentication(cookieOptions);

            return app.UseMiddleware<TokenMiddleware>(options);
        }
    }
}

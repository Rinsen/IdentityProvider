using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;

namespace Rinsen.IdentityProvider.Cookie
{
    public static class CookieAppBuilderExtensions
    {
        public static IApplicationBuilder UseCookieAuthentication(this IApplicationBuilder app)
        {
            if (app == null)
            {
                throw new ArgumentNullException(nameof(app));
            }

            return app.UseMiddleware<CookieMiddleware>();
        }

        public static IApplicationBuilder UseCookieAuthentication(this IApplicationBuilder app, CookieOptions options)
        {
            if (app == null)
            {
                throw new ArgumentNullException(nameof(app));
            }

            if (options == null)
            {
                throw new ArgumentNullException(nameof(options));
            }

            return app.UseMiddleware<CookieMiddleware>(options);
        }
    }
}

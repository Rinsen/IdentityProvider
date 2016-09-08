using Microsoft.AspNetCore.Builder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Rinsen.IdentityProvider.Client
{
    public static class RinsenAuthenticationAppBuilderExtensions
    {
        public static IApplicationBuilder UseRinsenAuthentication(this IApplicationBuilder app)
        {
            if (app == null)
            {
                throw new ArgumentNullException(nameof(app));
            }

            return app.UseMiddleware<RinsenAuthenticationMiddleware>();
        }

        public static IApplicationBuilder UseRinsenAuthentication(this IApplicationBuilder app, RinsenAuthenticationOptions options)
        {
            if (app == null)
            {
                throw new ArgumentNullException(nameof(app));
            }

            if (options == null)
            {
                throw new ArgumentNullException(nameof(options));
            }

            return app.UseMiddleware<RinsenAuthenticationMiddleware>(options);
        }

    }
}

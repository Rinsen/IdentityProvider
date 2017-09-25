using System;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Rinsen.IdentityProvider.Contracts;
using Microsoft.Extensions.Configuration;
using Rinsen.IdentityProvider.Core;
using System.Threading.Tasks;

namespace Rinsen.IdentityProvider.Token
{
    public static class TokenExtensions
    {
        public static AuthenticationBuilder AddTokenAuthentication(this IServiceCollection services, IConfiguration configuration)
        {
            return services.AddAuthentication(options =>
                 {
                     options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                 })
                    .AddToken(TokenDefaults.AuthenticationScheme, options =>
                    {

                        options.CallbackPath = new PathString("/token");
                        options.ClaimsIssuer = RinsenIdentityConstants.RinsenIdentityProvider;
                        options.ConnectionString = configuration["Data:DefaultConnection:ConnectionString"];
                        options.ApplicationKey = configuration["IdentityProvider:ApplicationKey"];
                        options.LoginPath = configuration["IdentityProvider:LoginPath"];
                        options.ValidateTokenPath = configuration["IdentityProvider:ValidateTokenPath"];
                    })
                    .AddCookie(options =>
                    {
                        options.SessionStore = new SqlTicketStore(new SessionStorage(configuration["Data:DefaultConnection:ConnectionString"]));
                        options.Events.OnRedirectToLogin = ctx => {
                            ctx.HttpContext.ChallengeAsync(TokenDefaults.AuthenticationScheme);

                            return Task.CompletedTask;
                        };
                    });
        }
        
        public static AuthenticationBuilder AddToken(this AuthenticationBuilder builder, string authenticationScheme, Action<TokenOptions> configureOptions)
            => builder.AddToken(authenticationScheme, authenticationScheme, configureOptions);


        public static AuthenticationBuilder AddToken(this AuthenticationBuilder builder, string authenticationScheme, string displayName, Action<TokenOptions> configureOptions)
        {
            builder.Services.TryAddEnumerable(ServiceDescriptor.Singleton<IPostConfigureOptions<TokenOptions>, TokenPostConfigureOptions>());
            return builder.AddRemoteScheme<TokenOptions, RemoteTokenHandler>(authenticationScheme, displayName, configureOptions);
        }
    }
}


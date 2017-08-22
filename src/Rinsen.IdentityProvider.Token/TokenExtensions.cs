using System;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.DependencyInjection;

namespace Rinsen.IdentityProvider.Token
{
    public static class TokenExtensions
    {

        public static AuthenticationBuilder AddToken(this AuthenticationBuilder authenticationBuilder, Action<TokenOptions> configureOptions)
        {
            authenticationBuilder.Services.Configure<AuthenticationOptions>(o =>
            {
                o.AddScheme(TokenDefaults.AuthenticationScheme, scheme =>
                {
                    scheme.HandlerType = typeof(TokenHandler);
                    scheme.DisplayName = TokenDefaults.AuthenticationScheme;
                });
            });
            if (configureOptions != null)
            {
                authenticationBuilder.Services.Configure(TokenDefaults.AuthenticationScheme, configureOptions);
            }
            authenticationBuilder.Services.AddTransient<TokenHandler>();
            return authenticationBuilder;
        }
    }
}


using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Rinsen.IdentityProvider.ExternalApplications;
using Rinsen.IdentityProvider.LocalAccounts;
using Rinsen.IdentityProvider.Core;
using System;

namespace Rinsen.IdentityProvider
{
    public static class ExtensionMethods
    {
        public static void AddRinsenIdentity(this IServiceCollection services, Action<IdentityOptions> identityOptionsAction)
        {
            var identityOptions = new IdentityOptions();

            identityOptionsAction.Invoke(identityOptions);

            services.AddSingleton(identityOptions);
            services.TryAddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            services.AddScoped<IIdentityService, IdentityService>();
            services.AddScoped<PasswordHashGenerator, PasswordHashGenerator>();
            services.AddScoped<IIdentityAccessor, IdentityAccessService>();
            services.AddScoped<ILocalAccountService, LocalAccountService>();
            services.AddScoped<ILoginService, LoginService>();
            services.AddScoped<IExternalApplicationService, ExternalApplicationService>();

            services.AddScoped<ILocalAccountStorage, LocalAccountStorage>();
            services.AddScoped<IIdentityStorage, IdentityStorage>();
            services.AddScoped<ISessionStorage, SessionStorage>();
            services.AddScoped<IExternalApplicationStorage, ExternalApplicationStorage>();
            services.AddScoped<ITokenStorage, TokenStorage>();
        }
    }
}

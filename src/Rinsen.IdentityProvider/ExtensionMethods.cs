using Microsoft.Extensions.DependencyInjection;
using Rinsen.IdentityProvider.Core;
using Rinsen.IdentityProvider.Core.LocalAccounts;
using Rinsen.IdentityProvider.Core.Sessions;
using System;

namespace Rinsen.IdentityProvider
{
    public static class ExtensionMethods
    {
        public static void AddRinsenIdentity(this IServiceCollection services, Action<IdentityOptions> identityOptionsAction)
        {
            services.AddRinsenIdentityCore(identityOptionsAction);

            services.AddTransient<ILocalAccountStorage, LocalAccountStorage>();
            services.AddTransient<IIdentityStorage, IdentityStorage>();
            services.AddTransient<ISessionStorage, SessionStorage>();
        }
    }
}

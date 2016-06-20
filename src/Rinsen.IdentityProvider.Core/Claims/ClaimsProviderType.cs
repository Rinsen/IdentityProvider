using System;
using System.Linq;
using System.Reflection;

namespace Rinsen.IdentityProvider.Core.Claims
{
    public class ClaimsProviderType
    {
        public ClaimsProviderType(Type type)
        {
            if (!type.GetTypeInfo().ImplementedInterfaces.Any(i => i == typeof(IClaimsProvider)))
            {
                throw new ArgumentException(string.Format("Type {0} must be derived from {1}", type.FullName, typeof(IClaimsProvider).FullName), nameof(type));
            }

            Type = type;
        }

        public Type Type { get; private set; }

    }
}

using System;
using System.Collections.ObjectModel;

namespace Rinsen.IdentityProvider.Claims
{
    public class ClaimsProviderCollection : Collection<ClaimsProviderType>
    {
        public void Add(Type type)
        {
            Add(new ClaimsProviderType(type));
        }

    }
}

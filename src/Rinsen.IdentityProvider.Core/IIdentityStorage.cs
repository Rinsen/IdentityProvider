using System;

namespace Rinsen.IdentityProvider.Core
{
    public interface IIdentityStorage
    {
        Guid Create(Identity identity);
        Identity Get(Guid identityId);
        void Disable(Guid identityId);
    }
}
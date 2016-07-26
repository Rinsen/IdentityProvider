using System;

namespace Rinsen.IdentityProvider.Core
{
    public interface IIdentityStorage
    {
        void Create(Identity identity);
        Identity Get(Guid identityId);
    }
}
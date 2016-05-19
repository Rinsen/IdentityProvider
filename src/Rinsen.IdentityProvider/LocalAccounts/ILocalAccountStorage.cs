using System;

namespace Rinsen.IdentityProvider.LocalAccounts
{
    public interface ILocalAccountStorage
    {
        void Create(LocalAccount localAccount);
        LocalAccount Get(Guid identityId);
        void Update(LocalAccount localAccount);
        void Delete(LocalAccount localAccount);
    }
}

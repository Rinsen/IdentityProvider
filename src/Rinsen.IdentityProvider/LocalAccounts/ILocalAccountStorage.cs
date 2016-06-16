using System;

namespace Rinsen.IdentityProvider.LocalAccounts
{
    public interface ILocalAccountStorage
    {
        void Create(LocalAccount localAccount);
        LocalAccount Get(Guid identityId);
        LocalAccount Get(string userName);
        void Update(LocalAccount localAccount);
        void Delete(LocalAccount localAccount);
        void UpdateFailedLoginCount(LocalAccount localAccount);
    }
}

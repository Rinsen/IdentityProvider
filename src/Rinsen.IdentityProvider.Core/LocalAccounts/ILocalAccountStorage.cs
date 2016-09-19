using System;
using System.Threading.Tasks;

namespace Rinsen.IdentityProvider.Core.LocalAccounts
{
    public interface ILocalAccountStorage
    {
        Task CreateAsync(LocalAccount localAccount);
        LocalAccount Get(Guid identityId);
        LocalAccount Get(string loginId);
        void Update(LocalAccount localAccount);
        void Delete(LocalAccount localAccount);
        void UpdateFailedLoginCount(LocalAccount localAccount);
    }
}

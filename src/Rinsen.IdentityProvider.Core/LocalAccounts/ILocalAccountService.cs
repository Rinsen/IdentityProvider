using System;
using System.Threading.Tasks;

namespace Rinsen.IdentityProvider.Core.LocalAccounts
{
    public interface ILocalAccountService
    {
        Task<CreateLocalAccountResult> CreateAsync(Guid identityId, string loginId, string password);
        Guid GetIdentityId(string loginId, string password);
        void ChangePassword(string oldPassword, string newPassword);
        void DeleteLocalAccount(string password);
        void ValidatePassword(string password);
    }
}

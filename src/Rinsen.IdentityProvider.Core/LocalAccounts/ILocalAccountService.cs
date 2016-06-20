using System;

namespace Rinsen.IdentityProvider.Core.LocalAccounts
{
    public interface ILocalAccountService
    {
        void CreateLocalAccount(Guid identityId, string loginId, string password);
        Guid GetIdentityId(string loginId, string password);
        void ChangePassword(string oldPassword, string newPassword);
        void DeleteLocalAccount(string password);
        void ValidatePassword(string password);
    }
}

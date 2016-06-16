using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Rinsen.IdentityProvider.LocalAccounts
{
    public interface ILocalAccountService
    {
        void CreateLocalAccount(Guid identityId, string userName, string password);
        Guid GetIdentityId(string userName, string password);
        void ChangeUserPassword(string oldPassword, string newPassword);
        void DeleteLocalAccount(string password);
    }
}

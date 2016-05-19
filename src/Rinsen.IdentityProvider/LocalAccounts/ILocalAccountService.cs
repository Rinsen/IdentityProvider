using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Rinsen.IdentityProvider.LocalAccounts
{
    public interface ILocalAccountService
    {
        void CreateLocalAccount(LocalAccount localAccount, string password);
        LocalAccount GetLocalAccount(string userEmail, string password);
        bool ChangeUserPassword(string oldPassword, string newPassword);
        void DeleteLocalAccount(string password);
    }
}

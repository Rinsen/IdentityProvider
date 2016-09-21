using Rinsen.IdentityProvider.Core.LocalAccounts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Rinsen.IdentityProvider.Core
{
    public class LoginService : ILoginService
    {
        private readonly ILocalAccountService _localAccountService;

        public Task<LoginResult> LoginAsync(string email, string password, bool rememberMe)
        {
            _localAccountService.GetIdentityIdAsync(email, password);

            throw new NotImplementedException();
        }

        public Task LogoutAsync()
        {
            throw new NotImplementedException();
        }
    }
}

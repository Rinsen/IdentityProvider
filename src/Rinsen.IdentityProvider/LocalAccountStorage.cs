using Rinsen.IdentityProvider.Core.LocalAccounts;
using System;

namespace Rinsen.IdentityProvider
{

    public class LocalAccountStorage : ILocalAccountStorage
    {
        public LocalAccountStorage()
        {
        }

        public void Create(LocalAccount localAccount)
        {
            throw new NotImplementedException();
        }

        public void Delete(LocalAccount localAccount)
        {
            throw new NotImplementedException();
        }

        public LocalAccount Get(string loginId)
        {
            throw new NotImplementedException();
        }

        public LocalAccount Get(Guid identityId)
        {
            throw new NotImplementedException();
        }

        public void Update(LocalAccount localAccount)
        {
            throw new NotImplementedException();
        }

        public void UpdateFailedLoginCount(LocalAccount localAccount)
        {
            throw new NotImplementedException();
        }
    }
}

using Rinsen.IdentityProvider.Core;
using System;

namespace Rinsen.IdentityProvider
{
    public class IdentityStorage : IIdentityStorage
    {
        public Guid Create(Identity identity)
        {
            throw new NotImplementedException();
        }

        public void Disable(Guid identityId)
        {
            throw new NotImplementedException();
        }

        public Identity Get(Guid identityId)
        {
            throw new NotImplementedException();
        }
    }
}

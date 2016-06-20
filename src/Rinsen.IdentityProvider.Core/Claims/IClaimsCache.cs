using System;
using System.Collections.Generic;
using System.Security.Claims;

namespace Rinsen.IdentityProvider.Core.Claims
{
    public interface IClaimsCache
    {
        bool TryGet(Guid identityId, out IEnumerable<Claim> claims);

        void Add(IEnumerable<Claim> claims);
    }
}

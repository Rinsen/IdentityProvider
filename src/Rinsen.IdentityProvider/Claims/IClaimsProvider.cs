using System;
using System.Collections.Generic;
using System.Security.Claims;

namespace Rinsen.IdentityProvider.Claims
{
    public interface IClaimsProvider
    {
        IEnumerable<Claim> GetClaims(Guid identityId);
    }
}

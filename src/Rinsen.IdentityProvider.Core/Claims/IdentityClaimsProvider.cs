using System;
using System.Collections.Generic;
using System.Security.Claims;

namespace Rinsen.IdentityProvider.Core.Claims
{
    public class IdentityClaimsProvider : IClaimsProvider
    {
        readonly IIdentityStorage _identityStorage;

        public IdentityClaimsProvider(IIdentityStorage identityStorage)
        {
            _identityStorage = identityStorage;
        }

        public IEnumerable<Claim> GetClaims(Guid identityId)
        {
            var identity = _identityStorage.Get(identityId);

            var claimsList = new List<Claim>
            {
                new Claim(System.Security.Claims.ClaimTypes.Name, identity.FirstName + " " + identity.LastName),
                new Claim(ClaimTypes.IdentityId, identity.IdentityId.ToString()),
            };

            return claimsList;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Security.Claims;

namespace Rinsen.IdentityProvider.Claims
{
    public class UserClaimsProvider : IClaimsProvider
    {
        readonly IIdentityStorage _userStorage;

        public UserClaimsProvider(IIdentityStorage userStorage)
        {
            _userStorage = userStorage;
        }

        public IEnumerable<Claim> GetClaims(Guid identityId)
        {
            var user = _userStorage.Get(identityId);

            var claimsList = new List<Claim>
            {
                new Claim(System.Security.Claims.ClaimTypes.Name, user.FirstName + " " + user.LastName),
                new Claim(ClaimTypes.IdentityId, user.IdentityId.ToString()),
            };

            return claimsList;
        }
    }
}

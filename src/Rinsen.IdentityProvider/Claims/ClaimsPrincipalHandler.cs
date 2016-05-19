using Rinsen.IdentityProvider.Sessions;
using System;
using System.Collections.Generic;
using System.Security.Claims;

namespace Rinsen.IdentityProvider.Claims
{
    public class ClaimsPrincipalHandler : IClaimsPrincipalHandler
    {
        readonly ClaimsProvidersFactory _claimsProviders;
        readonly IClaimsCache _claimsCache;


        public ClaimsPrincipalHandler(ClaimsProvidersFactory claimsProviders, IClaimsCache claimsCache)
        {
            _claimsProviders = claimsProviders;
            _claimsCache = claimsCache;
        }

        public ClaimsPrincipal GetClaimsPrincipal(Session session)
        {
            IEnumerable<Claim> cachedClaims;
            ClaimsIdentity identity;

            if (_claimsCache.TryGet(session.IdentityId, out cachedClaims))
            {
                identity = new ClaimsIdentity(cachedClaims, "Session");
            }
            else
            {
                var claims = GetClaimsList(session.IdentityId);

                claims.Add(new Claim(ClaimTypes.SessionId, session.Id));

                _claimsCache.Add(claims);

                identity = new ClaimsIdentity(claims, "Session");
            }

            return new ClaimsPrincipal(identity);
        }

        List<Claim> GetClaimsList(Guid identityId)
        {
            var claimsList = new List<Claim>();

            foreach (var claimProvider in _claimsProviders.GetClaimsProviders())
            {
                claimsList.AddRange(claimProvider.GetClaims(identityId));
            }

            return claimsList;
        }
    }
}

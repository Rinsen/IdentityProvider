using Rinsen.IdentityProvider.Core.Claims;
using Rinsen.IdentityProvider.Core.Sessions;
using System.Security.Claims;

namespace Rinsen.IdentityProvider.Core
{
    public class IdentityProvider
    {
        readonly ISessionStorage _sessionStorage;
        readonly IClaimsPrincipalHandler _claimsPrincipalHandler;

        public IdentityProvider(ISessionStorage sessionStorage, IClaimsPrincipalHandler claimsPrincipalFactory)
        {
            _sessionStorage = sessionStorage;
            _claimsPrincipalHandler = claimsPrincipalFactory;
        }

        public bool TryGetClaimsPrincipalFromSessionId(string sessionId, out ClaimsPrincipal claimsPrincipal)
        {
            var session = _sessionStorage.Get(sessionId);

            if (session != default(Session))
            {
                claimsPrincipal = _claimsPrincipalHandler.GetClaimsPrincipal(session);
                return true;
            }

            claimsPrincipal = null;
            return false;
        }
    }
}

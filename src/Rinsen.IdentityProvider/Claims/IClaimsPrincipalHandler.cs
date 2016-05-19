using Rinsen.IdentityProvider.Sessions;
using System;
using System.Security.Claims;

namespace Rinsen.IdentityProvider.Claims
{
    public interface IClaimsPrincipalHandler
    {
        ClaimsPrincipal GetClaimsPrincipal(Session session);
    }
}
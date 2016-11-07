using System;
using System.Security.Claims;

namespace Rinsen.IdentityProvider
{
    public interface IIdentityAccessor
    {
        ClaimsPrincipal ClaimsPrincipal { get; }
        //string SessionId { get; }
        Guid IdentityId { get; }
    }
}

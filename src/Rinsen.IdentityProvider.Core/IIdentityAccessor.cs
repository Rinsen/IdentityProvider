using System;
using System.Security.Claims;

namespace Rinsen.IdentityProvider.Core
{
    public interface IIdentityAccessor
    {
        ClaimsPrincipal ClaimsPrincipal { get; }
        //string SessionId { get; }
        Guid IdentityId { get; }
    }
}

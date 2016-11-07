using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using System;
using Rinsen.IdentityProvider.Core;

namespace Rinsen.IdentityProvider
{
    public class IdentityAccessService : IIdentityAccessor
    {
        readonly IHttpContextAccessor _httpContextAccessor;

        public IdentityAccessService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public ClaimsPrincipal ClaimsPrincipal
        {
            get
            {
                return _httpContextAccessor.HttpContext.User;
            }
        }

        //public string SessionId
        //{
        //    get
        //    {
        //        return ClaimsPrincipal.GetClaimStringValue(ClaimTypes.NameIdentifier);
        //    }
        //}

        public Guid IdentityId
        {
            get
            {
                return ClaimsPrincipal.GetClaimGuidValue(ClaimTypes.NameIdentifier);
            }
        }
    }
}

using Rinsen.IdentityProvider;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Rinsen.IdentityProvider.LocalAccounts;
using System.Security.Claims;

namespace Rinsen.IdentityProviderWeb.IdentityExtensions
{
    public class IdentityWebLoginService : LoginService
    {
        private readonly IIdentityAttributeStorage _identityAttributeStorage;
        public IdentityWebLoginService(ILocalAccountService localAccountService,
            IIdentityService identityService,
            IHttpContextAccessor httpContextAccessor,
            IIdentityAttributeStorage identityAttributeStorage)
            : base(localAccountService, identityService, httpContextAccessor)
        {
            _identityAttributeStorage = identityAttributeStorage;

        }

        protected override async Task AddApplicationSpecificClaimsAsync(List<Claim> claims)
        {
            var identityAttributes = await _identityAttributeStorage.GetIdentityAttributesAsync(Guid.Parse(claims.First(c => c.Type == ClaimTypes.NameIdentifier).Value));

            if (identityAttributes.Any(m => m.Attribute == "Administrator"))
            {
                claims.Add(new Claim("http://rinsen.se/Administrator", "True"));
            }
        }
    }
}
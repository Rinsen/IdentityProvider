using Rinsen.IdentityProvider.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Rinsen.IdentityProvider.Core.LocalAccounts;
using System.Security.Claims;

namespace Rinsen.IdentityProviderWeb.IdentityExtensions
{
    public class IdentityWebLoginService : LoginService
    {
        private readonly AdministratorStorage _administratorStorage;
        public IdentityWebLoginService(ILocalAccountService localAccountService,
            IIdentityService identityService,
            IHttpContextAccessor httpContextAccessor,
            AdministratorStorage administratorStorage)
            : base(localAccountService, identityService, httpContextAccessor)
        {
            _administratorStorage = administratorStorage;

        }

        protected override async Task AddApplicationSpecificClaimsAsync(List<Claim> claims)
        {
            var administrator = await _administratorStorage.GetAsync(Guid.Parse(claims.First(c => c.Type == ClaimTypes.NameIdentifier).Value));

            if (administrator != default(Administrator))
            {
                claims.Add(new Claim("http://rinsen.se/Administrator", "True"));
            }
        }
    }
}

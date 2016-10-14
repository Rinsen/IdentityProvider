using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Authentication;
using Rinsen.IdentityProvider.Core.LocalAccounts;
using Rinsen.IdentityProvider.Core.Sessions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Rinsen.IdentityProvider.Core
{
    public class LoginService : ILoginService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IIdentityService _identityService;
        private readonly ILocalAccountService _localAccountService;

        public LoginService(ILocalAccountService localAccountService,
            IIdentityService identityService,
            IHttpContextAccessor httpContextAccessor)
        {
            _localAccountService = localAccountService;
            _httpContextAccessor = httpContextAccessor;
            _identityService = identityService;
        }

        public async Task<LoginResult> LoginAsync(string email, string password, bool rememberMe)
        {
            var identityId = await _localAccountService.GetIdentityIdAsync(email, password);

            if (identityId == null)
            {
                return LoginResult.Failure();
            }

            var identity = await _identityService.GetIdentityAsync((Guid)identityId);

            var claims = await GetClaimsForIdentityAsync(identity);

            var authenticationProperties = new AuthenticationProperties()
            {
                AllowRefresh = true,
                ExpiresUtc = DateTimeOffset.UtcNow.AddMonths(1),
                IsPersistent = rememberMe,
                IssuedUtc = DateTimeOffset.UtcNow,
                RedirectUri = "?"
            };

            await _httpContextAccessor.HttpContext.Authentication.SignInAsync("RinsenCookie", new ClaimsPrincipal(new ClaimsIdentity(claims, "RinsenPassword")), authenticationProperties);

            return LoginResult.Success(identity);

        }

        private async Task<List<Claim>> GetClaimsForIdentityAsync(Identity identity)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, identity.GivenName + " " + identity.Surname, ClaimValueTypes.String, "RinsenIdentityProvider"),
                new Claim(ClaimTypes.NameIdentifier, identity.IdentityId.ToString(), ClaimValueTypes.String, "RinsenIdentityProvider"),
                new Claim(ClaimTypes.Email, identity.Email, ClaimValueTypes.String, "RinsenIdentityProvider"),
                new Claim(ClaimTypes.GivenName, identity.GivenName, ClaimValueTypes.String, "RinsenIdentityProvider"),
                new Claim(ClaimTypes.Surname, identity.Surname, ClaimValueTypes.String, "RinsenIdentityProvider")
            };

            await AddApplicationSpecificClaimsAsync(claims);

            return claims;
        }

        protected virtual Task AddApplicationSpecificClaimsAsync(List<Claim> claims)
        {
            return Task.CompletedTask;
        }

        public Task LogoutAsync()
        {
            throw new NotImplementedException();
        }
    }
}

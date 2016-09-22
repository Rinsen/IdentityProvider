using Microsoft.AspNetCore.Http;
using Rinsen.IdentityProvider.Core.LocalAccounts;
using Rinsen.IdentityProvider.Core.Sessions;
using System;
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

            var claims = new[]
            {
                new Claim(ClaimTypes.Name, identity.FirstName + " " + identity.LastName, ClaimValueTypes.String, "RinsenIdentityProvider"),
                new Claim(ClaimTypes.NameIdentifier, identity.IdentityId.ToString(), ClaimValueTypes.String, "RinsenIdentityProvider"),
                new Claim(ClaimTypes.Email, identity.Email, ClaimValueTypes.String, "RinsenIdentityProvider"),
                new Claim(ClaimTypes.GivenName, identity.FirstName, ClaimValueTypes.String, "RinsenIdentityProvider"),
                new Claim(ClaimTypes.Surname, identity.LastName, ClaimValueTypes.String, "RinsenIdentityProvider")
            };

            await _httpContextAccessor.HttpContext.Authentication.SignInAsync("RinsenCookie", new ClaimsPrincipal(new ClaimsIdentity(claims, "RinsenPassword")));

            return LoginResult.Success();
        }

        public Task LogoutAsync()
        {
            throw new NotImplementedException();
        }
    }
}

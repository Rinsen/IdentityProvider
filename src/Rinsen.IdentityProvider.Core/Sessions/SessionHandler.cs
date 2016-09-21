using Microsoft.AspNetCore.Http;
using Rinsen.IdentityProvider.Core.LocalAccounts;
using System;
using System.Threading.Tasks;

namespace Rinsen.IdentityProvider.Core.Sessions
{
    public class SessionHandler : ISessionHandler
    {
        readonly ILocalAccountService _localAccountService;
        readonly IdentityOptions _identityOptions;
        readonly IIdentityAccessor _claimsPrincipalAccessor;
        readonly IRandomDataGenerator _randomDataGenerator;
        readonly ISessionStorage _sessionStorage;
        readonly IHttpContextAccessor _httpContextAccessor;

        public SessionHandler(ILocalAccountService localAccountService, IdentityOptions identityOptions, IIdentityAccessor claimsPrincipalAccessor, IRandomDataGenerator randomDataGenerator, ISessionStorage sessionStorage, IHttpContextAccessor httpContextAccessor)
        {
            _localAccountService = localAccountService;
            _identityOptions = identityOptions;
            _claimsPrincipalAccessor = claimsPrincipalAccessor;
            _randomDataGenerator = randomDataGenerator;
            _sessionStorage = sessionStorage;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<string> CreateSessionAsync(string email, string password)
        {
            var session = new Session
            {
                Id = _randomDataGenerator.GetRandomString(_identityOptions.SessionIdLength),
                CreatedTimestamp = DateTimeOffset.Now,
                CreatedFromIpAddress = _httpContextAccessor.HttpContext.GetClientIPAddress(),
                LastUsed = DateTimeOffset.Now,
                LastUsedFromIpAddress = _httpContextAccessor.HttpContext.GetClientIPAddress(),
                IdentityId = await _localAccountService.GetIdentityIdAsync(email, password)
            };

            _sessionStorage.Create(session);

            return session.Id;
        }

        public void DeleteSession()
        {
            _sessionStorage.Delete(_claimsPrincipalAccessor.SessionId);
        }
    }
}

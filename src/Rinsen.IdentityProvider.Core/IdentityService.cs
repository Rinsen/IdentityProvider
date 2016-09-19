using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Rinsen.IdentityProvider.Core.LocalAccounts;
using System;
using System.Threading.Tasks;

namespace Rinsen.IdentityProvider.Core
{
    public class IdentityService : IIdentityService
    {
        readonly IIdentityAccessor _claimsPrincipalAccessor;
        readonly IdentityOptions _identityOptions;
        readonly IIdentityStorage _identityStorage;
        readonly ILocalAccountService _localAccountService;
        readonly ILogger _log;
        readonly IHttpContextAccessor _httpContextAccessor;

        public IdentityService(IIdentityAccessor claimsPrincipalAccessor,
            IdentityOptions identityOptions,
            IIdentityStorage identityStorage,
            ILocalAccountService localAccountService,
            ILogger<IdentityService> log,
            IHttpContextAccessor httpContextAccessor)
        {
            _claimsPrincipalAccessor = claimsPrincipalAccessor;
            _identityOptions = identityOptions;
            _identityStorage = identityStorage;
            _localAccountService = localAccountService;
            _log = log;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<CreateIdentityResult> CreateAsync(string firstName, string lastName, string email, string phoneNumber)
        {
            var identity = new Identity
            {
                IdentityId = Guid.NewGuid(),
                Created = DateTimeOffset.Now,
                Email = email,
                EmailConfirmed = false,
                FirstName = firstName,
                LastName = lastName,
                PhoneNumber = phoneNumber,
                PhoneNumberConfirmed = false,
                Updated = DateTimeOffset.Now
            };

            try
            {
                await _identityStorage.CreateAsync(identity);
            }
            catch (IdentityAlreadyExistException e)
            {
                _log.LogWarning("Identity {0} already exist from address {1}", identity.Email, _httpContextAccessor.HttpContext.GetClientIPAddressString());
                throw e;
            }
            _log.LogInformation("New identity created for email {0}, with name {1}, {2} and phone number {3}", identity.Email, identity.FirstName, identity.LastName, identity.PhoneNumber);
        }
        
        public Identity GetIdentity()
        {
            return _identityStorage.Get(_claimsPrincipalAccessor.IdentityId);
        }

        public Identity GetIdentity(Guid identityId)
        {
            return _identityStorage.Get(identityId);
        }

        public void UpdateIdentityDetails(string firstName, string lastName, string email, string phoneNumber)
        {
            throw new NotImplementedException();
        }

        public Task<CreateIdentityResult> CreateAsync(string firstName, string lastName, string email, string phoneNumber)
        {
            throw new NotImplementedException();
        }
    }
}

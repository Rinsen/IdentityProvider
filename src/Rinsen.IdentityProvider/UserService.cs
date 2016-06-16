using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;

namespace Rinsen.IdentityProvider
{
    public class UserService : IIdentityService
    {
        readonly IIdentityAccessor _claimsPrincipalAccessor;
        readonly IdentityOptions _identityOptions;
        readonly IIdentityStorage _identityStorage;
        readonly ILogger _log;
        readonly IHttpContextAccessor _httpContextAccessor;

        public UserService(IIdentityAccessor claimsPrincipalAccessor,
            IdentityOptions identityOptions,
            IIdentityStorage identityStorage,
            ILogger<UserService> log,
            IHttpContextAccessor httpContextAccessor)
        {
            _claimsPrincipalAccessor = claimsPrincipalAccessor;
            _identityOptions = identityOptions;
            _identityStorage = identityStorage;
            _log = log;
            _httpContextAccessor = httpContextAccessor;
        }

        public Guid CreateUser(Identity identity)
        {
            identity.Created = DateTimeOffset.Now;
            identity.Updated = DateTimeOffset.Now;

            Guid newIdentityId;
            try
            {
                newIdentityId = _identityStorage.Create(identity);
            }
            catch (UserAlreadyExistException e)
            {
                _log.LogWarning("User {0} already exist from address {1}", identity.Email, _httpContextAccessor.HttpContext.GetClientIPAddressString());
                throw e;
            }

            return newIdentityId;
        }

        public Identity GetUser()
        {
            return _identityStorage.Get(_claimsPrincipalAccessor.IdentityId);
        }

        public Identity GetUser(Guid identityId)
        {
            return _identityStorage.Get(identityId);
        }

        public void UpdateUserDetails(string firstName, string lastName, string email, string phoneNumber)
        {
            throw new NotImplementedException();
        }
    }
}

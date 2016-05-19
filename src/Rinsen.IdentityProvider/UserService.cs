using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;

namespace Rinsen.IdentityProvider
{
    public class UserService : IIdentityService
    {
        readonly IIdentityAccessor _claimsPrincipalAccessor;
        readonly PasswordHashGenerator _passwordHashGenerator;
        readonly IRandomDataGenerator _randomDataGenerator;
        readonly IdentityOptions _identityOptions;
        readonly IIdentityStorage _userStorage;
        readonly ILogger _log;
        readonly IHttpContextAccessor _httpContextAccessor;

        public UserService(IIdentityAccessor claimsPrincipalAccessor,
            IdentityOptions identityOptions,
            PasswordHashGenerator passwordHashGenerator,
            IRandomDataGenerator randomDataGenerator,
            IIdentityStorage userStorage,
            ILoggerFactory loggerFactory,
            IHttpContextAccessor httpContextAccessor)
        {
            _claimsPrincipalAccessor = claimsPrincipalAccessor;
            _passwordHashGenerator = passwordHashGenerator;
            _randomDataGenerator = randomDataGenerator;
            _identityOptions = identityOptions;
            _userStorage = userStorage;
            _log = loggerFactory.CreateLogger<UserService>();
            _httpContextAccessor = httpContextAccessor;
        }

        public Guid CreateUser(Identity identity)//, string userPassword)
        {
            //user.IterationCount = _identityOptions.IterationCount;
            //user.PasswordSalt = _randomDataGenerator.GetRandomByteArray(_identityOptions.NumberOfBytesInPasswordSalt);
            //user.PasswordHash = _passwordHashGenerator.GetPasswordHash(user.PasswordSalt, userPassword, user.IterationCount, _identityOptions.NumberOfBytesInPasswordHash);

            identity.Created = DateTimeOffset.Now;
            identity.Updated = DateTimeOffset.Now;

            Guid newIdentityId;
            try
            {
                newIdentityId = _userStorage.Create(identity);
            }
            catch (UserAlreadyExistException e)
            {
                _log.LogWarning("User {0} already exist from address {1}", identity.Email, _httpContextAccessor.HttpContext.GetClientIPAddressString());
                throw e;
            }

            return newIdentityId;
        }

        //public Identity GetUser(string userEmail, string userPassword)
        //{
        //    if (string.IsNullOrEmpty(userEmail))
        //    {
        //        throw new ArgumentException("No user email provided");
        //    }

        //    if (string.IsNullOrEmpty(userPassword))
        //    {
        //        throw new ArgumentException("No user password provided");
        //    }

        //    var user = _userStorage.Get(userEmail);

        //    if (user == default(Identity))
        //    {
        //        return null;
        //    }

        //    // validate password against password hash and salt
        //    if (IsPasswordValid(user, userPassword))
        //    {
        //        return user;
        //    }

        //    return null;
        //}

        public Identity GetUser()
        {
            return _userStorage.Get(_claimsPrincipalAccessor.IdentityId);
        }

        public Identity GetUser(Guid identityId)
        {
            return _userStorage.Get(identityId);
        }

        //bool IsPasswordValid(Identity user, string userPassword)
        //{
        //    var isValid = false;

        //    if (user.FailedLoginCount > 0)
        //    {
        //        if (user.FailedLoginCount < _identityOptions.MaxFailedLoginSleepCount)
        //        {
        //            Task.Delay(TimeSpan.FromSeconds(user.FailedLoginCount)).Wait();
        //        }
        //        else
        //        {
        //            _log.LogWarning("Max failed logins done for user {0}, {1} from IP {2}", user.UserId, user.Email, _httpContextAccessor.HttpContext.GetClientIPAddressString());
        //            Task.Delay(TimeSpan.FromSeconds(_identityOptions.MaxFailedLoginSleepCount)).Wait();
        //        }
        //    }

        //    var passwordHash = _passwordHashGenerator.GetPasswordHash(user.PasswordSalt, userPassword, user.IterationCount, _identityOptions.NumberOfBytesInPasswordHash);

        //    if (user.PasswordHash.SequenceEqual(passwordHash))
        //    {
        //        isValid = true;
        //    }

        //    if (!isValid)
        //    {
        //        AddToFailedLoginCount(user);
        //    }
        //    else if (user.FailedLoginCount > 0)
        //    {
        //        SetFailedLoginCountToZero(user);
        //    }

        //    return isValid;
        //}

        //void AddToFailedLoginCount(Identity user)
        //{
        //    user.FailedLoginCount++;
        //    user.Updated = DateTimeOffset.Now;

        //    _userStorage.UpdateFailedLoginCount(user);
        //}

        //void SetFailedLoginCountToZero(Identity user)
        //{
        //    user.FailedLoginCount = 0;
        //    user.Updated = DateTimeOffset.Now;

        //    _userStorage.UpdateFailedLoginCount(user);
        //}

        //public bool ChangeUserPassword(string oldPassword, string newPassword)
        //{
        //    var user = GetUser();

        //    if (IsPasswordValid(user, oldPassword))
        //    {
        //        user.IterationCount = _identityOptions.IterationCount;
        //        user.PasswordSalt = _randomDataGenerator.GetRandomByteArray(_identityOptions.NumberOfBytesInPasswordSalt);
        //        user.PasswordHash = _passwordHashGenerator.GetPasswordHash(user.PasswordSalt, newPassword, user.IterationCount, _identityOptions.NumberOfBytesInPasswordHash);
        //        user.Updated = DateTimeOffset.Now;
        //        _userStorage.UpdatePasswordAndSalt(user);
        //        return true;
        //    }
        //    return false;
        //}

        public void UpdateUserDetails(string firstName, string lastName, string email, string phoneNumber)
        {
            throw new NotImplementedException();
        }
    }
}

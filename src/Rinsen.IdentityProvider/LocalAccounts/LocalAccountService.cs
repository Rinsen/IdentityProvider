using Microsoft.Extensions.Logging;
using System;

namespace Rinsen.IdentityProvider.LocalAccounts
{
    public class LocalAccountService : ILocalAccountService
    {
        private readonly IIdentityAccessor _identityAccessor;
        private readonly ILocalAccountStorage _localAccountStorage;
        private readonly IdentityOptions _options;
        private readonly PasswordHashGenerator _passwordHashGenerator;
        private readonly IRandomDataGenerator _randomDataGenerator;
        private readonly ILogger<LocalAccountService> _log;

        public LocalAccountService(IIdentityAccessor identityAccessor,
            ILocalAccountStorage localAccountStorage,
            IdentityOptions options,
            IRandomDataGenerator randomDataGenerator,
            PasswordHashGenerator passwordHashGenerator,
            ILogger<LocalAccountService> log)
        {
            _localAccountStorage = localAccountStorage;
            _identityAccessor = identityAccessor;
            _options = options;
            _randomDataGenerator = randomDataGenerator;
            _passwordHashGenerator = passwordHashGenerator;
            _identityAccessor = identityAccessor;
            _log = log;
        }


        public void ChangePassword(string oldPassword, string newPassword)
        {
            var localAccount = GetLocalAccount(oldPassword);

            localAccount.PasswordHash = GetPasswordHash(newPassword, localAccount);
            localAccount.Updated = DateTimeOffset.Now;

            _localAccountStorage.Update(localAccount);
        }

        public void CreateLocalAccount(Guid identityId, string loginId, string password)
        {
            var localAccount = new LocalAccount
            {
                IdentityId = identityId,
                IterationCount = _options.IterationCount,
                PasswordSalt = _randomDataGenerator.GetRandomByteArray(_options.NumberOfBytesInPasswordSalt),
                LoginId = loginId,
                Created = DateTimeOffset.Now,
                Updated = DateTimeOffset.Now
            };
            localAccount.PasswordHash = GetPasswordHash(password, localAccount);

            _localAccountStorage.Create(localAccount);
        }

        private byte[] GetPasswordHash(string password, LocalAccount localAccount)
        {
            return _passwordHashGenerator.GetPasswordHash(localAccount.PasswordSalt, password, localAccount.IterationCount, _options.NumberOfBytesInPasswordHash);
        }

        public void DeleteLocalAccount(string password)
        {
            var localAccount = GetLocalAccount(password);

            _localAccountStorage.Delete(localAccount);
        }

        private LocalAccount GetLocalAccount(string password)
        {
            var localAccount = _localAccountStorage.Get(_identityAccessor.IdentityId);

            ValidatePassword(localAccount, password);

            return localAccount;
        }

        public Guid GetIdentityId(string loginId, string password)
        {
            var localAccount = _localAccountStorage.Get(loginId);

            ValidatePassword(localAccount, password);

            if (localAccount.FailedLoginCount > 0)
            {
                SetFailedLoginCountToZero(localAccount);
            }

            return localAccount.IdentityId;
        }

        private void SetFailedLoginCountToZero(LocalAccount localAccount)
        {
            localAccount.FailedLoginCount = 0;
            localAccount.Updated = DateTimeOffset.Now;
            _localAccountStorage.UpdateFailedLoginCount(localAccount);
        }

        private void InvalidPassword(LocalAccount localAccount)
        {
            localAccount.FailedLoginCount++;
            localAccount.Updated = DateTimeOffset.Now;

            _log.LogWarning("Invalid password for local account {0} with iteration count {1}", localAccount.IdentityId, localAccount.IterationCount);

            _localAccountStorage.UpdateFailedLoginCount(localAccount);
            
            throw new UnauthorizedAccessException("Invalid password");
        }

        public void ValidatePassword(string password)
        {
            var localAccount = _localAccountStorage.Get(_identityAccessor.IdentityId);
            ValidatePassword(localAccount, password);
        }

        private void ValidatePassword(LocalAccount localAccount, string password)
        {
            if (localAccount.PasswordHash != GetPasswordHash(password, localAccount))
            {
                InvalidPassword(localAccount);
            }
        }
    }
}

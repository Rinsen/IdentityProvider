using Rinsen.IdentityProvider.Core.Claims;
using System;

namespace Rinsen.IdentityProvider.Core
{
    public class IdentityOptions
    {
        public IdentityOptions()
        {
            SessionCookieOnlySecureTransfer = true;
            IterationCount = 10000;
            NumberOfBytesInPasswordHash = 128 / 8;
            NumberOfBytesInPasswordSalt = 128 / 8;
            SessionKeyName = "rkey";
            SessionIdLength = 40;
            MaxFailedLoginSleepCount = 30;
            ClaimsProviders = new ClaimsProviderCollection();
            ClaimsCacheTimeout = new TimeSpan(0, 30, 0);
            ClaimsProviders.Add(typeof(IdentityClaimsProvider));
        }

        public string ConnectionString { get; set; }

        public TimeSpan ClaimsCacheTimeout { get; set; }

        public int IterationCount { get; set; }

        public int NumberOfBytesInPasswordSalt { get; set; }

        public int NumberOfBytesInPasswordHash { get; set; }

        public bool SessionCookieOnlySecureTransfer { get; set; }

        public int SessionIdLength { get; set; }

        public string SessionKeyName { get; set; }

        public int MaxFailedLoginSleepCount { get; set; }

        public ClaimsProviderCollection ClaimsProviders { get; private set; }

    }
}

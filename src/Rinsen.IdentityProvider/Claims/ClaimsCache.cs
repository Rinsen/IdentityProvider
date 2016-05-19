using System;

namespace Rinsen.IdentityProvider.Sessions
{
    public class ClaimsCache
    {
        public int UserId { get; set; }

        public DateTimeOffset Created { get; set; }

        public string SerializedClaims { get; set; }

    }
}

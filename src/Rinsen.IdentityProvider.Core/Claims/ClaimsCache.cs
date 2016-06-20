using System;

namespace Rinsen.IdentityProvider.Core.Sessions
{
    public class ClaimsCache
    {
        public int IdentityId { get; set; }

        public DateTimeOffset Created { get; set; }

        public string SerializedClaims { get; set; }

    }
}

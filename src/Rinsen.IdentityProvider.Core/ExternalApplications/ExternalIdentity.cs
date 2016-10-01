using System;

namespace Rinsen.IdentityProvider.Core.ExternalApplications
{
    public class ExternalIdentity
    {
        public Guid IdentityId { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Issuer { get; set; }


    }
}

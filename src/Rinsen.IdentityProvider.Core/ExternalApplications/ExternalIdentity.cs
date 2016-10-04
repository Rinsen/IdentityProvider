using System;

namespace Rinsen.IdentityProvider.Core.ExternalApplications
{
    public class ExternalIdentity
    {
        public Guid IdentityId { get; set; }

        public string GivenName { get; set; }

        public string Surname { get; set; }

        public string Issuer { get; set; }


    }
}

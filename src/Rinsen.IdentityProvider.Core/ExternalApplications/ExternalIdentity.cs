using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Rinsen.IdentityProvider.Core.ExternalApplications
{
    public class ExternalIdentity
    {
        public Guid IdentityId { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

    }
}

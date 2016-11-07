using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Rinsen.IdentityProvider.ExternalApplications
{
    public class Token
    {
        public int ClusteredId { get; set; }
        public string TokenId { get; set; }
        public Guid ExternalApplicationId { get; set; }
        public DateTimeOffset Created { get; set; }
        public Guid IdentityId { get; set; }

    }
}

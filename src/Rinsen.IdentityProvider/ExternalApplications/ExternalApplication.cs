using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Rinsen.IdentityProvider.ExternalApplications
{
    public class ExternalApplication
    {
        public int Id { get; set; }
        public Guid ExternalApplicationId { get; set; }
        public string ApplicationKey { get; set; }
        public string Hostname { get; set; }
        public bool Active { get; set; }
        public DateTimeOffset ActiveUntil { get; set; }


    }
}

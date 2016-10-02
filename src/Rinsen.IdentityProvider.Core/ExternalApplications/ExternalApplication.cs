﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Rinsen.IdentityProvider.Core.ExternalApplications
{
    public class ExternalApplication
    {
        public int ClusteredId { get; set; }
        public Guid ExternalApplicationId { get; set; }
        public string Password { get; set; }
        public string HostName { get; set; }
        public bool Active { get; set; }

    }
}

﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Rinsen.IdentityProvider.Core.ExternalApplications
{
    public class Token
    {
        public string TokenId { get; set; }
        public Guid ExternalApplicationId { get; set; }
        public DateTimeOffset Created { get; set; }
        public Guid IdentityId { get; set; }

    }
}

﻿using Rinsen.IdentityProvider.ExternalApplications;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Rinsen.IdentityProviderWeb.Areas.WebApi.Models
{
    public class ExternalApplicationsResult
    {
        public IEnumerable<ExternalApplication> ExternalApplications { get; set; }


    }
}

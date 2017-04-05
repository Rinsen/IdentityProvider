﻿using System;

namespace Rinsen.IdentityProviderWeb.Areas.WebApi.Models
{
    public class ExternalApplicationToCreate
    {
        public string HostName { get; set; }

        public bool Active { get; set; }

        public DateTimeOffset ActiveUntil { get; set; }

    }
}
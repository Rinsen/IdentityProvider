﻿using System;

namespace Rinsen.IdentityProvider.Core.ExternalApplications.v1
{
    public class ExternalIdentityResult
    {
        public Guid IdentityId { get; set; }
        public string GivenName { get; set; }
        public string Surname { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string Issuer { get; set; }
    }
}
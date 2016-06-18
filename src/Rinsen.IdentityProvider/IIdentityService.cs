﻿using System;

namespace Rinsen.IdentityProvider
{
    public interface IIdentityService
    {
        void CreateIdentity(Identity identity);
        Identity GetIdentity(Guid identityId);
        Identity GetIdentity();
        void UpdateIdentityDetails(string firstName, string lastName, string email, string phoneNumber);
    }
}

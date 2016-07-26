using System;

namespace Rinsen.IdentityProvider.Core
{
    public interface IIdentityService
    {
        void CreateIdentity(Identity identity, string loginId, string password);
        Identity GetIdentity(Guid identityId);
        Identity GetIdentity();
        void UpdateIdentityDetails(string firstName, string lastName, string email, string phoneNumber);
    }
}

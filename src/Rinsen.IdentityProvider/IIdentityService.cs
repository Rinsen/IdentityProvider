using System;

namespace Rinsen.IdentityProvider
{
    public interface IIdentityService
    {
        Guid CreateUser(Identity identity);
        Identity GetUser(Guid identityId);
        Identity GetUser();
        void UpdateUserDetails(string firstName, string lastName, string email, string phoneNumber);
    }
}

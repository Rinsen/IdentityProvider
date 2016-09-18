using System;
using System.Threading.Tasks;

namespace Rinsen.IdentityProvider.Core
{
    public interface IIdentityService
    {
        Identity GetIdentity(Guid identityId);
        Identity GetIdentity();
        void UpdateIdentityDetails(string firstName, string lastName, string email, string phoneNumber);
        Task<CreateIdentityResult> CreateAsync(string firstName, string lastName, string email, string phoneNumber);
    }
}

using System;
using System.Threading.Tasks;

namespace Rinsen.IdentityProvider.ExternalApplications
{
    public interface IExternalApplicationService
    {
        Task<ValidationResult> GetTokenForValidHostAsync(string applicationName, string host, Guid identityId, Guid correlationId, bool rememberMe);
        Task<IdentityResult> GetTokenAsync(string tokenId, string applicationKey);
    }
}

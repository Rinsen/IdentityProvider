using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Rinsen.IdentityProvider.ExternalApplications
{
    public interface IExternalApplicationService
    {
        Task<ValidationResult> GetTokenForValidHostAsync(string host, Guid identityId);
        Task<IdentityResult> GetIdentityForTokenAndApplicationKeyAsync(string tokenId, string applicationKey);
    }
}

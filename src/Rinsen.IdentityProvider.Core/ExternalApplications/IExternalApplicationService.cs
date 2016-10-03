using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Rinsen.IdentityProvider.Core.ExternalApplications
{
    public interface IExternalApplicationService
    {
        Task<ValidationResult> GetTokenForValidHostAsync(string returnUrl, Guid identityId);
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Rinsen.IdentityProvider.Core.ExternalApplications
{
    public interface IExternalApplicationService
    {
        ValidationResult ValidateAsync(string returnUrl);
    }
}

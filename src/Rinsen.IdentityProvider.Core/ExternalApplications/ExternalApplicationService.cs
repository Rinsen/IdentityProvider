using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Rinsen.IdentityProvider.Core.ExternalApplications
{
    public class ExternalApplicationService : IExternalApplicationService
    {


        public ValidationResult ValidateAsync(string returnUrl)
        {
            return ValidationResult.Failure();
        }
    }
}

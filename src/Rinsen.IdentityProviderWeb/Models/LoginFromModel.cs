using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace Rinsen.IdentityProviderWeb.Models
{
    public class LoginFromModel : IValidatableObject
    {
        public string ReturnTo { get; set; }

        public string Host { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (string.IsNullOrEmpty(ReturnTo) && string.IsNullOrEmpty(Host))
                return Enumerable.Empty<ValidationResult>();

            var validationResult = new List<ValidationResult>();

            if (string.IsNullOrEmpty(ReturnTo))
            {
                validationResult.Add(new ValidationResult("Value is not provided", new[] { nameof(ReturnTo) }));
                return validationResult;
            }

            if (string.IsNullOrEmpty(Host))
            {
                validationResult.Add(new ValidationResult("Value is not provided", new[] { nameof(Host) }));
                return validationResult;
            }

            var returnToUrl = new Uri(ReturnTo);

            if (returnToUrl.Host != Host)
            {
                validationResult.Add(new ValidationResult("Values does not match", new[] { nameof(ReturnTo), nameof(Host) }));
            }

            return validationResult;
        }
    }
}

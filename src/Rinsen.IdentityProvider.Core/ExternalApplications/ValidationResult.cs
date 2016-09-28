using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Primitives;

namespace Rinsen.IdentityProvider.Core.ExternalApplications
{
    public class ValidationResult
    {
        public bool Succeeded { get; set; }
        public bool Failed { get; set; }
        public StringValues Token { get; set; }

        public static ValidationResult Failure()
        {
            return new ValidationResult() { Failed = true };
        }

        public static ValidationResult Success(string token)
        {
            return new ValidationResult() { Succeeded = true, Token = token };
        }
    }
}

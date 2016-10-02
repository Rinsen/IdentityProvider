using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Primitives;

namespace Rinsen.IdentityProvider.Core.ExternalApplications
{
    public class ValidationResult
    {
        private bool _succeeded;

        public bool Succeeded { get { return _succeeded; } }
        public bool Failed { get { return !_succeeded; } }
        public string Token { get; private set; }

        public static ValidationResult Failure()
        {
            return new ValidationResult() { _succeeded = false };
        }

        public static ValidationResult Success(string token)
        {
            return new ValidationResult() { _succeeded = true, Token = token };
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Rinsen.IdentityProvider.Core
{
    public class CreateIdentityResult
    {
        public CreateIdentityResult()
        {
        }

        public CreateIdentityResult(Identity identity)
        {
            Identity = identity;
        }

        public Identity Identity { get; }
        public bool Succeeded { get; private set; }
        public bool IdentityAlreadyExist { get; private set; }

        public static CreateIdentityResult AlreadyExist()
        {
            return new CreateIdentityResult() { IdentityAlreadyExist = true };
        }

        public static CreateIdentityResult Success(Identity identity)
        {
            return new CreateIdentityResult(identity) { Succeeded = true };
        }
    }
}

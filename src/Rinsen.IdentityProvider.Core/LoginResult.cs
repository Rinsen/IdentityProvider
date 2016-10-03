using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Rinsen.IdentityProvider.Core
{
    public class LoginResult
    {
        public Identity Identity { get; private set; }
        public bool Succeeded { get; private set; }
        public bool Failed { get { return !Succeeded; } }

        public static LoginResult Failure()
        {
            return new LoginResult() { Succeeded = false };
        }

        public static LoginResult Success(Identity identity)
        {
            return new LoginResult() { Succeeded = true, Identity = identity };
        }
    }
}

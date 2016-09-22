using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Rinsen.IdentityProvider.Core
{
    public class LoginResult
    {
        public LoginResult()
        {
        }
        
        public bool Succeeded { get; private set; }
        public bool Failed { get; private set; }

        public static LoginResult Failure()
        {
            return new LoginResult() { Failed = true };
        }

        public static LoginResult Success()
        {
            return new LoginResult() { Succeeded = true };
        }
    }
}

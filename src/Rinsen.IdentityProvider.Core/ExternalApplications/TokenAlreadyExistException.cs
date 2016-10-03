using System;

namespace Rinsen.IdentityProvider.Core
{
    public class TokenAlreadyExistException : Exception
    {
        public TokenAlreadyExistException(string message, Exception innerException)
            : base(message, innerException)
        {

        }
    }
}

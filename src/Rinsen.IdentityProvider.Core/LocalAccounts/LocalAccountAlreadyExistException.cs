using System;

namespace Rinsen.IdentityProvider.Core
{
    public class LocalAccountAlreadyExistException : Exception
    {
        public LocalAccountAlreadyExistException(string message, Exception innerException)
            : base(message, innerException)
        {

        }
    }
}

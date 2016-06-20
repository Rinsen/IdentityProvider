using System;

namespace Rinsen.IdentityProvider.Core
{
    public class IdentityAlreadyExistException : Exception
    {
        public IdentityAlreadyExistException(string message, Exception innerException)
            : base(message, innerException)
        {

        }
    }
}

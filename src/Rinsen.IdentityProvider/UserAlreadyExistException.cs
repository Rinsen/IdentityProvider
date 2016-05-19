using System;

namespace Rinsen.IdentityProvider
{
    public class UserAlreadyExistException : Exception
    {
        public UserAlreadyExistException(string message, Exception innerException)
            : base(message, innerException)
        {

        }
    }
}

using System;

namespace Rinsen.IdentityProvider.Sessions
{
    public class SessionAlreadyExistException : Exception
    {
        public SessionAlreadyExistException(string message, Exception innerException)
            :base(message, innerException)
        {

        }

    }
}

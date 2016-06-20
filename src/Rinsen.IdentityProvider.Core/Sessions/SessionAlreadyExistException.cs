using System;

namespace Rinsen.IdentityProvider.Core.Sessions
{
    public class SessionAlreadyExistException : Exception
    {
        public SessionAlreadyExistException(string message, Exception innerException)
            :base(message, innerException)
        {

        }

    }
}

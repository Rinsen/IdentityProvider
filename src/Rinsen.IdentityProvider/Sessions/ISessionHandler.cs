using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Rinsen.IdentityProvider.Sessions
{
    public interface ISessionHandler
    {
        /// <summary>
        /// Create new SessionId for user session
        /// </summary>
        /// <param name="userEmail"></param>
        /// <param name="password"></param>
        /// <returns>Session id</returns>
        string CreateSession(string userEmail, string password);
        void DeleteSession();
    }
}

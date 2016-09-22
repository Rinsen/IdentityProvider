using System;
using System.Threading.Tasks;

namespace Rinsen.IdentityProvider.Core.Sessions
{
    public interface ISessionHandler
    {
        /// <summary>
        /// Create new SessionId for identity session
        /// </summary>
        /// <param name="loginId"></param>
        /// <param name="password"></param>
        /// <returns>Session id</returns>
        Task<string> CreateSessionAsync(Guid id);
        void DeleteSession();
    }
}

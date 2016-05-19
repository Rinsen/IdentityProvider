using System.Collections.Generic;

namespace Rinsen.IdentityProvider.Sessions
{
    public interface ISessionStorage
    {
        void Create(Session session);

        int Delete(string sessionId);

        IEnumerable<Session> Get(int userId);

        Session Get(string sessionId);
    }
}

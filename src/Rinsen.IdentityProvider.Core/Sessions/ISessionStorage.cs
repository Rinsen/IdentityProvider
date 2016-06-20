using System.Collections.Generic;

namespace Rinsen.IdentityProvider.Core.Sessions
{
    public interface ISessionStorage
    {
        void Create(Session session);

        int Delete(string sessionId);

        IEnumerable<Session> Get(int loginId);

        Session Get(string sessionId);
    }
}

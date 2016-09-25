using System.Collections.Generic;
using System.Threading.Tasks;

namespace Rinsen.IdentityProvider.Core.Sessions
{
    public interface ISessionStorage
    {
        Task CreateAsync(Session session);

        int Delete(string sessionId);

        IEnumerable<Session> Get(int loginId);

        Session Get(string sessionId);
    }
}

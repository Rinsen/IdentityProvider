using Rinsen.IdentityProvider.Core.Sessions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Rinsen.IdentityProvider
{
    public class SessionStorage : ISessionStorage
    {
        public void Create(Session session)
        {
            throw new NotImplementedException();
        }

        public int Delete(string sessionId)
        {
            throw new NotImplementedException();
        }

        public Session Get(string sessionId)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Session> Get(int loginId)
        {
            throw new NotImplementedException();
        }
    }
}

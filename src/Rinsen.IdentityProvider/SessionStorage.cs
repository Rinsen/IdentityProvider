using Rinsen.IdentityProvider.Core.Sessions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Rinsen.IdentityProvider
{
    public class SessionStorage : ISessionStorage
    {
        private string _connectionString;
        
        public SessionStorage(string connectionString)
        {
            _connectionString = connectionString;
        }

        public Task CreateAsync(Session session)
        {
            throw new NotImplementedException();
        }

        public Task DeleteAsync(string sessionId)
        {
            throw new NotImplementedException();
        }

        public Task<Session> GetAsync(string sessionId)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Session>> GetAsync(Guid identityId)
        {
            throw new NotImplementedException();
        }
    }
}

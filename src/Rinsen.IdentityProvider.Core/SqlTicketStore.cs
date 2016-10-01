using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Rinsen.IdentityProvider.Core.Sessions;
using System.Security.Claims;
using System.Security.Cryptography;

namespace Rinsen.IdentityProvider.Core
{
    public class SqlTicketStore : ITicketStore
    {
        private readonly TicketSerializer _ticketSerializer = new TicketSerializer();
        private readonly ISessionStorage _sessionStorage;
        private readonly RandomNumberGenerator CryptoRandom = RandomNumberGenerator.Create();

        public SqlTicketStore(ISessionStorage sessionStorage)
        {
            _sessionStorage = sessionStorage;
        }

        public async Task RemoveAsync(string key)
        {
            await _sessionStorage.DeleteAsync(key);
        }

        public Task RenewAsync(string key, AuthenticationTicket ticket)
        {
            var serializedTicket = _ticketSerializer.Serialize(ticket);

            throw new NotImplementedException();
        }

        public async Task<AuthenticationTicket> RetrieveAsync(string key)
        {
            var session = await _sessionStorage.GetAsync(key);

            var ticket = _ticketSerializer.Deserialize(session.SerializedTicket);

            return ticket;
        }

        public async Task<string> StoreAsync(AuthenticationTicket ticket)
        {
            var bytes = new byte[32];
            CryptoRandom.GetBytes(bytes);
            var correlationId = Base64UrlTextEncoder.Encode(bytes);

            var session = new Session
            {
                SessionId = correlationId,
                IdentityId = ticket.Principal.GetClaimGuidValue(ClaimTypes.NameIdentifier),
                LastAccess = DateTimeOffset.Now,
                SerializedTicket = _ticketSerializer.Serialize(ticket)
            };

            await _sessionStorage.CreateAsync(session);

            return session.SessionId;
        }
    }
}
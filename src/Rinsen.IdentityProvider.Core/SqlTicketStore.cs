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

        public Task RemoveAsync(string key)
        {
            throw new NotImplementedException();
        }

        public Task RenewAsync(string key, AuthenticationTicket ticket)
        {
             var ticket2 = _ticketSerializer.Serialize(ticket);

            throw new NotImplementedException();
        }

        public Task<AuthenticationTicket> RetrieveAsync(string key)
        {
            throw new NotImplementedException();
        }

        public async Task<string> StoreAsync(AuthenticationTicket ticket)
        {
            var bytes = new byte[32];
            CryptoRandom.GetBytes(bytes);
            var correlationId = Base64UrlTextEncoder.Encode(bytes);

            var session = new Session
            {
                Id = correlationId,
                IdentityId = ticket.Principal.GetClaimGuidValue(ClaimTypes.NameIdentifier),
                LastAccess = DateTimeOffset.Now,
                SerializedTicket = _ticketSerializer.Serialize(ticket)
            };

            await _sessionStorage.CreateAsync(session);

            return session.Id;
        }
    }
}
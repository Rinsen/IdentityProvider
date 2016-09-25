using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;

namespace Rinsen.IdentityProvider.Core
{
    public class SqlTicketStore : ITicketStore
    {
        private readonly TicketSerializer _ticketSerializer;

        public SqlTicketStore()
        {
            _ticketSerializer = new TicketSerializer();
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

        public Task<string> StoreAsync(AuthenticationTicket ticket)
        {
            var ticket2 = _ticketSerializer.Serialize(ticket);
            throw new NotImplementedException();
        }
    }
}


// Session data

    //Id
    //Device Browser?
    //Ip
    //Last used DateTime, but only care about date
    //Created DateTime
    //Session data byte[]


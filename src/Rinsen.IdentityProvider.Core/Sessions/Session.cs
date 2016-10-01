using System;
using System.Net;

namespace Rinsen.IdentityProvider.Core.Sessions
{
    public class Session
    {
        public int ClusterId { get; set; }
        public string SessionId { get; set; }
        public Guid IdentityId { get; set; }
        public DateTimeOffset LastAccess { get; set; }
        public byte[] SerializedTicket { get; set; }

    }
}

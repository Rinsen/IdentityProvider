using System;
using System.Net;

namespace Rinsen.IdentityProvider.Sessions
{
    public class Session
    {
        public string Id { get; set; }

        public Guid IdentityId { get; set; }

        public DateTimeOffset LastUsed { get; set; }

        public string LastUsedFromIpAddressString { get; set; }

        public IPAddress LastUsedFromIpAddress { get { return IPAddress.Parse(LastUsedFromIpAddressString); } set { LastUsedFromIpAddressString = value.ToString(); } }

        public DateTimeOffset CreatedTimestamp { get; set; }

        public string CreatedFromIpAddressString { get; set; }

        public IPAddress CreatedFromIpAddress { get { return IPAddress.Parse(CreatedFromIpAddressString); } set { CreatedFromIpAddressString = value.ToString(); } }
        
    }
}

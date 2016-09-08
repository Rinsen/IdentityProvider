using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Rinsen.IdentityProvider.Client
{
    public class RinsenAuthenticationOptions : AuthenticationOptions, IOptions<RinsenAuthenticationOptions>
    {

        public RinsenAuthenticationOptions() : base()
        {
            AuthenticationScheme = RinsenAuthenticationDefaults.AuthenticationScheme;
            TokenKey = RinsenAuthenticationDefaults.TokenKey;

        }

        public string IdentityProviderBaseAddress { get; set; }

        public string TokenKey { get; set; }

        public RinsenAuthenticationOptions Value
        {
            get
            {
                return this;
            }
        }
    }
}

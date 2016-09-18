using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Rinsen.IdentityProviderWeb.Models
{
    public class CreateIdentityModel
    {
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PhoneNumber { get; set; }
        public string Password { get; set; }
        public string PasswordRepeated { get; set; }
        public string InviteCode { get; set; }
        public string ReturnUrl { get; internal set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Rinsen.IdentityProviderWeb.Models
{
    public class OldIdentityModel
    {
        public OldLoginModel LoginModel { get; set; }

        public OldCreateIdentityModel CreateIdentityModel { get; set; }

        public OldIdentityDetailsModel IdentityDetailsModel { get; set; }

        public OldChangePasswordModel ChangePasswordModel { get; set; }

        public bool IdentityAlreadyExist { get; set; }
    }
}

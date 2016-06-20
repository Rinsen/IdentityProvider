using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Rinsen.IdentityProviderWeb.Models
{
    public class IdentityModel
    {
        public LoginModel LoginModel { get; set; }

        public CreateIdentityModel CreateIdentityModel { get; set; }

        public IdentityDetailsModel IdentityDetailsModel { get; set; }

        public ChangePasswordModel ChangePasswordModel { get; set; }

        public bool IdentityAlreadyExist { get; set; }
    }
}

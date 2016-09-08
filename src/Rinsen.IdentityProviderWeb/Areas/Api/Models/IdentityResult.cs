using System;

namespace Rinsen.IdentityProviderWeb.Areas.Api.Models
{
    public class IdentityResult
    {
        public Guid Id { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Email { get; set; }

        public string PhoneNumber { get; set; }

    }
}

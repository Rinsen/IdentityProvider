using System.ComponentModel.DataAnnotations;

namespace Rinsen.IdentityProviderWeb.Models
{
    public class LoginModel
    {
        [Required]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }
        public bool RememberMe { get; set; }
        public string ApplicationName { get; set; }
        public string Host { get; set; }
        public string ExternalUrl { get; set; }
        public bool InvalidEmailOrPassword { get; set; }
        public string RedirectUrl { get;set; }
    }
}

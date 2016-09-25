using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Rinsen.IdentityProviderWeb.Models
{
    public class CreateIdentityModel : IValidatableObject
    {
        [Display(Name = "Email")]
        [DataType(DataType.EmailAddress)]
        [Required(ErrorMessage = "The email field is required")]
        public string Email { get; set; }

        [Display(Name = "Given name")]
        [Required(ErrorMessage = "The given name field is required")]
        public string GivenName { get; set; }

        [Display(Name = "Surname")]
        [Required(ErrorMessage = "The last name field is required")]
        public string Surname { get; set; }

        [Display(Name = "Phone number")]
        public string PhoneNumber { get; set; }

        [Display(Name = "Password")]
        [Required(ErrorMessage = "The password field is required")]
        public string Password { get; set; }
        [Display(Name = "Repeat password")]
        [Required(ErrorMessage = "The repeated password field is required")]
        public string PasswordRepeated { get; set; }

        [Display(Name = "Invite Code")]
        [Required(ErrorMessage = "Invite code is required")]
        public string InviteCode { get; set; }

        public string ReturnUrl { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            var validationList = new List<ValidationResult>();

            if (InviteCode != "freddeärbäst")
            {
                validationList.Add(new ValidationResult("Invalid invite code.", new[] { "InviteCode" }));
            }

            if (string.IsNullOrEmpty(Password) || string.IsNullOrEmpty(Password) || Password != PasswordRepeated)
            {
                validationList.Add(new ValidationResult("Passwords does not match", new[] { "Password", "PasswordRepeated" }));
            }

            return validationList;
        }
    }
}

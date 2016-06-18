using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Rinsen.IdentityWeb.Models
{
    public class ChangePasswordModel
    {
        [Display(Name = "Current password")]
        [Required(ErrorMessage = "The current password field is required")]
        public string CurrentPassword { get; set; }

        [Display(Name = "New password")]
        [Required(ErrorMessage = "The new password field is required")]
        public string Password { get; set; }

        [Display(Name = "Repeat new password")]
        [Required(ErrorMessage = "The repeated new password field is required")]
        public string PasswordRepeated { get; set; }

        public bool InvalidCurrentPassword { get; private set; }

        public bool PasswordUpdateSucceeded { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            var validationList = new List<ValidationResult>();

            if (CurrentPassword == Password)
            {
                validationList.Add(new ValidationResult("New password cannot be same as the old password", new[] { "CurrentPassword", "Password", "PasswordRepeated" }));
            }

            if (Password != PasswordRepeated)
            {
                validationList.Add(new ValidationResult("Passwords does not match", new[] { "Password", "PasswordRepeated" }));
            }

            return validationList;
        }

        public void SetValidPasswordState()
        {
            PasswordUpdateSucceeded = true;
            InvalidCurrentPassword = false;
            InsertEmptyValues();
        }

        public void SetInvalidCurrentPasswordState()
        {
            PasswordUpdateSucceeded = false;
            InvalidCurrentPassword = true;
            InsertEmptyValues();
        }

        private void InsertEmptyValues()
        {
            CurrentPassword = string.Empty;
            Password = string.Empty;
            PasswordRepeated = string.Empty;
        }
    }
}

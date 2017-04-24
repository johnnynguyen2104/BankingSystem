using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BankSystem.Models.AccountViewModels
{
    public class AccountCreationVM
    {
        public string UserId { get; set; }

        public string AccountNumber { get; set; } = Guid.NewGuid().ToString();

        [Required]
        [Display(Name = "Account's Name")]
        [StringLength(30, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
        public string AccountName { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }
    }
}

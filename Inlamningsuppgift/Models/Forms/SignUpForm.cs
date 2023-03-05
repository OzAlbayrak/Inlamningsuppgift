using System.ComponentModel.DataAnnotations;

namespace Inlamningsuppgift.Models.Forms
{
    public class SignUpForm
    {
        [Required]
        [Display(Name = "Your First Name")]
        public string FirstName { get; set; } = null!;

        [Required]
        [Display(Name = "Your Last Name")]
        public string LastName { get; set; } = null!;

        [Required]
        [EmailAddress]
        [Display(Name = "Your E-mail Address")]
        public string Email { get; set; } = null!;

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Your Password")]
        public string Password { get; set; } = null!;

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Confirm Password")]
        [Compare(nameof(Password))]
        public string ConfirmPassword { get; set; } = null!;

        [Required]
        [Display(Name = "Your Street Name")]
        public string StreetName { get; set; } = null!;

        [Required]
        [Display(Name = "Your Postal Code")]
        public string PostalCode { get; set; } = null!;

        [Required]
        [Display(Name = "Your City")]
        public string City { get; set; } = null!;

        public string? PhoneNumber { get; set; }

        public string? Company { get; set; }

        [Display(Name = "Add Your Photo")]
        public IFormFile? ProfileImage { get; set; }

        public string? ReturnUrl { get; set; }

        public bool TermsAndAggreements { get; set; }
    }
}

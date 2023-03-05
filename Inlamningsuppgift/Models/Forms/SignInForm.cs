using System.ComponentModel.DataAnnotations;

namespace Inlamningsuppgift.Models.Forms
{
    public class SignInForm
    {
        [Required]
        [EmailAddress]
        [Display(Name = "Your E-mail Address")]
        public string Email { get; set; } = null!;

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Your Password")]
        public string Password { get; set; } = null!;

        public bool KeepMeLoggedIn { get; set; }

        public string? ReturnUrl { get; set; }
    }
}

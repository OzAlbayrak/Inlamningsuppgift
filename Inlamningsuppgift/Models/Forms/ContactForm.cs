using System.ComponentModel.DataAnnotations;

namespace Inlamningsuppgift.Models.Forms
{
    public class ContactForm
    {
        [Required]
        [Display(Name = "Your Name")]
        public string Name { get; set; } = null!;

        [Required]
        [EmailAddress]
        [Display(Name = "Your E-mail Address")]
        public string Email { get; set; } = null!;

        [Required]
        [Display(Name = "Your Phone Number")]
        public string PhoneNumber { get; set; } = null!;

        public string? Company { get; set; }

        [Required]
        [Display(Name = "Write Someting")]
        public string WritenText { get; set; } = null!;

        public string? ReturnUrl { get; set; }

        public bool SaveMyData { get; set; }
    }
}

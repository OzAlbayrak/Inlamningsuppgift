using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Inlamningsuppgift.Models.Forms
{
    public class AddProductForm
    {
        [Required]
        [Display(Name = "Your Product Article Numbere")]
        public string ArticleNumber { get; set; } = null!;

        [Required]
        [Display(Name = "Your Product Name")]
        public string Name { get; set; } = null!;

        [Required]
        [Column(TypeName = "money")]
        [Display(Name = "Your Product Price")]
        public decimal Price { get; set; }

        [Display(Name = "Your Product Category")]
        public string? Category { get; set; }

        [Required]
        [Display(Name = "Your Product Rating")]
        public int Rating { get; set; }

       
        [Display(Name = "Your Short Product Description")]
        public string? ProductDescriptionShort { get; set; }

        
        [Display(Name = "Your Long Product Description")]
        public string? ProductDescriptionLong { get; set; }

        public IFormFile? ProductImageName { get; set; }
        public string? ReturnUrl { get; set; }
    }
}
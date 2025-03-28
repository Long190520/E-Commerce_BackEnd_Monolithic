using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace E_Commerce_BackEnd.Models
{
    public class Products : Entity
    {
        public Products()
        {
            ProductVariants = new HashSet<ProductVariants>();
        }

        [Required]
        public required string Name { get; set; }
        [Required]
        public required string Description { get; set; }
        public string? Image { get; set; }
        [Required]
        [ForeignKey(nameof(Categories))]
        public Guid CategoryId { get; set; }

        public required virtual Categories Category { get; set; }
        public virtual ICollection<ProductVariants> ProductVariants { get; set; } = new List<ProductVariants>();
        public virtual ICollection<ProductReviews> ProductReviews { get; set; } = new List<ProductReviews>();
    }
}

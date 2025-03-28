using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace E_Commerce_BackEnd.Models
{
    [Table("ProductReviews")]
    public class ProductReviews : Entity
    {
        public required string Title { get; set; }
        public required string Content { get; set; }

        [Range(0, 5, ErrorMessage = "Value must be between 0 and 5.")]
        public int Rating { get; set; }
        public string? Image { get; set; }

        [Required]
        [ForeignKey(nameof(Users))]
        public Guid UserId { get; set; }

        [Required]
        [ForeignKey(nameof(Products))]
        public Guid ProductId { get; set; }

        [Required]
        [ForeignKey(nameof(ProductVariants))]
        public Guid ProductVariantsId { get; set; }

        public required Users User { get; set; }
        public required Products Product { get; set; }
        public required ProductVariants ProductVariant { get; set; }
    }
}

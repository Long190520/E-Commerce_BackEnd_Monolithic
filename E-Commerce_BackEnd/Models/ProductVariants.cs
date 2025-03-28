using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace E_Commerce_BackEnd.Models
{
    public class ProductVariants : Entity
    {
        [Required]
        [ForeignKey(nameof(Products))]
        public Guid ProductId { get; set; }
        public required string Name { get; set; }
        public string? Description { get; set; }
        public int Stock { get; set; }
        public decimal Price { get; set; }
        public string? Image { get; set; }

        public required virtual Products Product { get; set; }
        public virtual ICollection<CartItems> CartItems { get; set; } = new List<CartItems>();
        public virtual ICollection<ProductReviews> ProductReviews { get; set; } = new List<ProductReviews>();
        public virtual ICollection<OrderDetails> OrderDetails { get; set; } = new List<OrderDetails>();
    }
}

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace E_Commerce_BackEnd.Models
{
    [Table("CartItems")]
    public class CartItems : Entity
    {
        [Required]
        [ForeignKey(nameof(Carts))]
        public Guid CartId { get; set; }
        [Required]
        [ForeignKey(nameof(ProductVariants))]
        public Guid ProductVariantId { get; set; }
        public int Quantity { get; set; }

        public required virtual Carts Cart { get; set; }
        public required virtual ProductVariants ProductVariant { get; set; }
    }
}

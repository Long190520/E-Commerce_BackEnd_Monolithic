using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace E_Commerce_BackEnd.Models
{
    public class OrderDetails : Entity
    {
        [Required]
        [ForeignKey(nameof(Orders))]
        public Guid OrderId { get; set; }

        [Required]
        [ForeignKey(nameof(ProductVariants))]
        public Guid ProductVariantId { get; set; }

        [Required]
        public int Quantity { get; set; }

        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal UnitPrice { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal TotalPrice => Quantity * UnitPrice;

        public virtual Orders Order { get; set; } = null!;
        public virtual ProductVariants ProductVariant { get; set; } = null!;
    }
}

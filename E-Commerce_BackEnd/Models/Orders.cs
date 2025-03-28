using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace E_Commerce_BackEnd.Models
{
    [Table("Orders")]
    public class Orders : Entity
    {
        public Orders()
        {
            OrderDetails = new HashSet<OrderDetails>();
        }

        [Required]
        [ForeignKey(nameof(Users))]
        public Guid UserId { get; set; }

        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal TotalAmount { get; set; }

        [Required]
        [MaxLength(50)]
        public string OrderStatus { get; set; } = "Pending"; // Default status

        public virtual Users User { get; set; } = null!;
        public virtual ICollection<OrderDetails> OrderDetails { get; set; } = new List<OrderDetails>();
    }
}

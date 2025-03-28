using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace E_Commerce_BackEnd.Models
{
    [Table("Carts")]
    public class Carts
    {
        public Carts()
        {
            CartItems = new HashSet<CartItems>();
        }

        [Key]
        public Guid UserId { get; set; }

        public virtual Users? User { get; set; }
        public virtual ICollection<CartItems> CartItems { get; set; } = new List<CartItems>();
    }
}

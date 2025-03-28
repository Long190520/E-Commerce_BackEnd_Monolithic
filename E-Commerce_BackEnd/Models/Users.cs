using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace E_Commerce_BackEnd.Models
{
    [Table("Users")]
    public class Users : IdentityUser<Guid>
    {
        [Required]
        public required string FirstName { get; set; }
        [Required]
        public required string LastName { get; set; }
        public string? RefreshToken { get; set; }
        public DateTime? RefreshTokenExp { get; set; }

        public virtual Carts? Cart { get; set; }
        public virtual ICollection<Orders> Orders { get; set; } = new List<Orders>();
        public virtual ICollection<ProductReviews> ProductReviews { get; set; } = new List<ProductReviews>();
    }
}

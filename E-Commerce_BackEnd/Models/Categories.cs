using System.ComponentModel.DataAnnotations.Schema;

namespace E_Commerce_BackEnd.Models
{
    [Table("Categories")]
    public class Categories : Entity
    {
        public Categories()
        {
            Products = new HashSet<Products>();
        }

        public required string Name { get; set; }
        public required string Description { get; set; }

        public virtual ICollection<Products> Products { get; set; } = new List<Products>();
    }
}

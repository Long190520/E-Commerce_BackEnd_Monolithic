using System.ComponentModel.DataAnnotations;

namespace E_Commerce_BackEnd.Models
{
    public class Entity
    {
        [Key]
        public Guid Id { get; set; }
        public DateTime? CreatedAt { get; set; }
        public string? CreatedBy { get; set; }
        public DateTime? LastModified { get; set; }
        public string? LastModifiedBy { get; set; }
    }
}

using System.ComponentModel.DataAnnotations;

namespace StoreManagementSystem.API.Models
{
    public class Supplier
    {
        [Key]
        public int SupplierId { get; set; }
        [Required]
        public string Name { get; set; } = string.Empty;
        public ICollection<Product> Products { get; set; } = new List<Product>();
    }
}

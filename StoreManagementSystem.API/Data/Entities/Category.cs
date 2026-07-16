using System.ComponentModel.DataAnnotations;

namespace StoreManagementSystem.API.Data.Entities
{
    public class Category
    {
        [Key]
        public int CategoryId { get; set; }
        [Required]
        public string Name { get; set; } = string.Empty;
        public ICollection<Product> Products { get; set; } = new List<Product>();
    }
}

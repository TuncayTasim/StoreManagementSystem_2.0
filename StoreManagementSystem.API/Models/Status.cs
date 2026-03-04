using System.ComponentModel.DataAnnotations;

namespace StoreManagementSystem.API.Models
{
    public class Status
    {
        [Key]
        public int StatusId { get; set; }
        [Required]
        public string Name { get; set; } = string.Empty; // e.g., Active, Inactive
        public ICollection<User> Users { get; set; } = new List<User>();
    }
}

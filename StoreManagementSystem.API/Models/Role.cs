using System.ComponentModel.DataAnnotations;

namespace StoreManagementSystem.API.Models
{
    public class Role
    {
        [Key]
        public int RoleId { get; set; }
        [Required]
        public string Name { get; set; } = string.Empty;
        public ICollection<User> Users { get; set; } = new List<User>();
    }
}

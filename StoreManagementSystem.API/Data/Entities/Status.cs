using System.ComponentModel.DataAnnotations;

namespace StoreManagementSystem.API.Data.Entities
{
    public class Status
    {
        [Key]
        public int StatusId { get; set; }
        [Required]
        public string Name { get; set; } = string.Empty;
        public ICollection<User> Users { get; set; } = new List<User>();
    }
}

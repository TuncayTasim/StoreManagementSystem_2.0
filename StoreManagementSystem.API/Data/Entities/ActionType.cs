using System.ComponentModel.DataAnnotations;

namespace StoreManagementSystem.API.Data.Entities
{
    public class ActionType
    {
        [Key]
        public int ActionId { get; set; }
        [Required]
        public string Name { get; set; } = string.Empty;
    }
}

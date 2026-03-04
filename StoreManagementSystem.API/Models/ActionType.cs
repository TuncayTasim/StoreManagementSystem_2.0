using System.ComponentModel.DataAnnotations;

namespace StoreManagementSystem.API.Models
{
    public class ActionType
    {
        [Key]
        public int ActionId { get; set; }
        [Required]
        public string Name { get; set; } = string.Empty; // e.g., Restock, MoveToShelf, Reject, Sell
    }
}

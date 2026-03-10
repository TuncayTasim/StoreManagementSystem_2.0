using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StoreManagementSystem.API.Models
{
    public class Rejection
    {
        [Key]
        public int Id { get; set; }
        public int? WarehouseId { get; set; }
        public int? ShelfId { get; set; }

        [Required]
        public string Reason { get; set; } = string.Empty;
    }
}

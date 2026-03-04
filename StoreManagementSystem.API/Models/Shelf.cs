using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StoreManagementSystem.API.Models
{
    public class Shelf
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey("ActionType")]
        public int ActionId { get; set; }
        public ActionType? ActionType { get; set; }

        [ForeignKey("Product")]
        public int ProductId { get; set; }
        public Product? Product { get; set; }

        public int Quantity { get; set; }
        public int CurrentQuantity { get; set; }
        public DateTime ActionDateTime { get; set; } = DateTime.Now;

        public ShelfRestock? RestockDetails { get; set; }
    }
}

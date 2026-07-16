using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StoreManagementSystem.API.Data.Entities
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

        public decimal Quantity { get; set; }
        public decimal CurrentQuantity { get; set; }
        public DateTime ActionDateTime { get; set; } = DateTime.Now;

        public ShelfRestock? RestockDetails { get; set; }
    }
}

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StoreManagementSystem.API.Models
{
    public class ShelfRestock
    {
        [Key]
        public int Id { get; set; }
        public decimal PriceSell { get; set; }

        [ForeignKey("Shelf")]
        public int ShelfId { get; set; }
        public Shelf? Shelf { get; set; }
    }
}

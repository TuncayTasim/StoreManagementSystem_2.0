using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StoreManagementSystem.API.Models
{
    public class Sale
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey("Shelf")]
        public int ShelfId { get; set; }
        public Shelf? Shelf { get; set; }

        public decimal PriceSold { get; set; }
        public decimal QuantitySold { get; set; }
        public string PaymentMethod { get; set; } = "Cash";
    }
}

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StoreManagementSystem.API.Data.Entities
{
    public class Product
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;

        [ForeignKey("Category")]
        public int CategoryId { get; set; }
        public Category? Category { get; set; }

        [ForeignKey("Supplier")]
        public int SupplierId { get; set; }
        public Supplier? Supplier { get; set; }

        public string SKU { get; set; } = string.Empty;
        public string Barcode { get; set; } = string.Empty;

        public decimal WarehouseQuantity { get; set; }
        public decimal ShelfQuantity { get; set; }

        public ICollection<Warehouse> WarehouseActions { get; set; } = new List<Warehouse>();
        public ICollection<Shelf> ShelfActions { get; set; } = new List<Shelf>();
    }
}

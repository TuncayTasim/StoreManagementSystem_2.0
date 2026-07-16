using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StoreManagementSystem.API.Models
{
    public class WarehouseRestock
    {
        [Key]
        public int Id { get; set; }
        public decimal PriceBought { get; set; }
        public int DaysToExpire { get; set; }

        [ForeignKey("Warehouse")]
        public int WarehouseId { get; set; }
        public Warehouse? Warehouse { get; set; }

        [NotMapped]
        public DateTime? ExpirationDate => Warehouse?.ActionDateTime.AddDays(DaysToExpire);
    }
}

namespace StoreManagementSystem.API.DTOs
{
    public class RejectionDTO
    {
        public int Id { get; set; } // This will be the WarehouseId or ShelfId
        public string? Reason { get; set; }
    }
}

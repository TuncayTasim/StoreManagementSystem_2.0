namespace StoreManagementSystem.API.DTOs
{
    public class RejectionDTO
    {
        public int Id { get; set; }
        public string? Reason { get; set; }
    }
    public record RejectionExternalInfoDTO(string SourceType, string ProductName, DateTime? RejectedAt);
    public record RejectionFullInfoDTO(int Id, string? Reason, string SourceType, string ProductName, DateTime? RejectedAt);
}

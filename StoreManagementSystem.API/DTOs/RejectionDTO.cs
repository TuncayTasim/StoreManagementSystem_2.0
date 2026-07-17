namespace StoreManagementSystem.API.DTOs
{
    public record RejectionDTO(int Id, string? Reason);
    public record RejectionExternalInfoDTO(string SourceType, string ProductName, DateTime? RejectedAt);
    public record RejectionFullInfoDTO(int Id, string? Reason, string SourceType, string ProductName, DateTime? RejectedAt);
}

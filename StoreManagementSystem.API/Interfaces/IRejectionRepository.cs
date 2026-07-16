using StoreManagementSystem.API.Data.Entities;
using StoreManagementSystem.API.DTOs;

namespace StoreManagementSystem.API.Interfaces
{
    public interface IRejectionRepository
    {
        Task AddRejectionAsync(Rejection rejection);
        Task<IEnumerable<Rejection>> GetAllRejectionsAsync();
        Task<RejectionExternalInfoDTO> GetShelfBatch(Rejection r, string sourceType, string productName, DateTime? rejectedAt);
        Task<RejectionExternalInfoDTO> GetWarehouseBatch(Rejection r, string sourceType, string productName, DateTime? rejectedAt);
        Task SaveChangesAsync();
    }
}

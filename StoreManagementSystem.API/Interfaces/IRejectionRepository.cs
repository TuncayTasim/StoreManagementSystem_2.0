using StoreManagementSystem.API.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace StoreManagementSystem.API.Interfaces
{
    public interface IRejectionRepository
    {
        Task AddRejectionAsync(Rejection rejection);
        Task<IEnumerable<Rejection>> GetAllRejectionsAsync();
        Task SaveChangesAsync();
    }
}

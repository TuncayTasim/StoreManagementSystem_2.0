using StoreManagementSystem.API.Models;
using System.Threading.Tasks;

namespace StoreManagementSystem.API.Repositories
{
    public interface IRejectionRepository
    {
        Task AddRejectionAsync(Rejection rejection);
        Task SaveChangesAsync();
    }
}

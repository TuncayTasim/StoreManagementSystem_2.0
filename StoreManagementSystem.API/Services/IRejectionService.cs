using System.Collections.Generic;
using System.Threading.Tasks;

namespace StoreManagementSystem.API.Services
{
    public interface IRejectionService
    {
        Task<IEnumerable<object>> GetAllRejectionsAsync();
    }
}
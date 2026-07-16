using StoreManagementSystem.API.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace StoreManagementSystem.API.Interfaces
{
    public interface IRejectionService
    {
        Task<IEnumerable<RejectionFullInfoDTO>> GetAllRejectionsAsync();
    }
}
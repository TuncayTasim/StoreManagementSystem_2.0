using StoreManagementSystem.API.Data;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System;
using System.Threading.Tasks;
using StoreManagementSystem.API.Interfaces;
using StoreManagementSystem.API.Repositories;
using StoreManagementSystem.API.DTOs;

namespace StoreManagementSystem.API.Services
{
    public class RejectionService : IRejectionService
    {
        private readonly IRejectionRepository _repository;

        public RejectionService(IRejectionRepository repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<RejectionFullInfoDTO>> GetAllRejectionsAsync()
        {
            var rejections = await _repository.GetAllRejectionsAsync();
            var result = new List<RejectionFullInfoDTO>();

            foreach (var r in rejections)
            {
                string sourceType = "Unknown";
                string productName = "Unknown";
                DateTime? rejectedAt = null;

                if (r.ShelfId.HasValue)
                {
                    var shelfInfo = await _repository.GetShelfBatch(r, sourceType, productName, rejectedAt);
                    sourceType = shelfInfo.SourceType;
                    productName = shelfInfo.ProductName;
                    rejectedAt = shelfInfo.RejectedAt;
                }
                else if (r.WarehouseId.HasValue)
                {
                    var warehouseInfo = await _repository.GetWarehouseBatch(r, sourceType, productName, rejectedAt);
                    sourceType = warehouseInfo.SourceType;
                    productName = warehouseInfo.ProductName;
                    rejectedAt = warehouseInfo.RejectedAt;
                }

                result.Add(new RejectionFullInfoDTO(r.Id, r.Reason, sourceType, productName, rejectedAt));
            }

            return result;
        }
    }
}
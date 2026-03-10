using Microsoft.AspNetCore.Mvc;
using StoreManagementSystem.API.Services;
using Microsoft.AspNetCore.Authorization;

namespace StoreManagementSystem.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class InventoryController : ControllerBase
    {
        private readonly IWarehouseService _warehouseService;
        private readonly IShelfService _shelfService;
        private readonly ISalesService _salesService;
        private readonly IRejectionService _rejectionService;

        public InventoryController(IWarehouseService warehouseService, IShelfService shelfService, ISalesService salesService, IRejectionService rejectionService)
        {
            _warehouseService = warehouseService;
            _shelfService = shelfService;
            _salesService = salesService;
            _rejectionService = rejectionService;
        }

        [HttpGet("rejections")]
        public async Task<IActionResult> GetRejections()
        {
            try
            {
                var rejections = await _rejectionService.GetAllRejectionsAsync();
                return Ok(rejections);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("warehouse/history")]
        public async Task<IActionResult> GetWarehouseHistory([FromQuery] int? productId = null)
        {
            try
            {
                var history = await _warehouseService.GetAllHistoryAsync(productId);
                return Ok(history);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("shelf/history")]
        public async Task<IActionResult> GetShelfHistory([FromQuery] int? productId = null)
        {
            try
            {
                var history = await _shelfService.GetAllHistoryAsync(productId);
                return Ok(history);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("restock")]
        public async Task<IActionResult> Restock(int productId, decimal quantity, decimal price, int daysToExpire)
        {
            try
            {
                await _warehouseService.RestockProductAsync(productId, quantity, price, daysToExpire);
                return Ok("Product restocked in warehouse.");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("move-to-shelf")]
        [Authorize(Roles = "Admin,Shelf Manager")]
        public async Task<IActionResult> MoveToShelf(int productId, decimal quantity, decimal sellPrice)
        {
            try
            {
                await _shelfService.MoveToShelfAsync(productId, quantity, sellPrice);
                return Ok("Product moved to shelf.");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("sell")]
        [Authorize(Roles = "Admin,Sales Manager")]
        public async Task<IActionResult> Sell(int productId, decimal quantity, string paymentMethod)
        {
            try
            {
                await _salesService.ProcessSaleAsync(productId, quantity, paymentMethod);
                return Ok("Sale processed");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("warehouse/batches/{productId}")]
        public async Task<IActionResult> GetWarehouseBatches(int productId)
        {
            try
            {
                var batches = await _warehouseService.GetWarehouseBatchesAsync(productId);
                return Ok(batches);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("shelf/batches/{productId}")]
        public async Task<IActionResult> GetShelfBatches(int productId)
        {
            try
            {
                var batches = await _shelfService.GetShelfBatchesAsync(productId);
                return Ok(batches);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("reject/warehouse")]
        public async Task<IActionResult> RejectFromWarehouse([FromBody] DTOs.RejectionDTO rejectionDto)
        {
            try
            {
                var reason = rejectionDto.Reason ?? "No reason provided.";
                await _warehouseService.RejectWarehouseBatchAsync(rejectionDto.Id, reason);
                return Ok("Warehouse batch rejected successfully.");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("reject/shelf")]
        public async Task<IActionResult> RejectFromShelf([FromBody] DTOs.RejectionDTO rejectionDto)
        {
            try
            {
                var reason = rejectionDto.Reason ?? "No reason provided.";
                await _shelfService.RejectShelfBatchAsync(rejectionDto.Id, reason);
                return Ok("Shelf batch rejected successfully.");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}

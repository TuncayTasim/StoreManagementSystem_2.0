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

        public InventoryController(IWarehouseService warehouseService, IShelfService shelfService, ISalesService salesService)
        {
            _warehouseService = warehouseService;
            _shelfService = shelfService;
            _salesService = salesService;
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
        public async Task<IActionResult> Restock(int productId, int quantity, decimal price, int daysToExpire)
        {
            try
            {
                await _warehouseService.RestockProductAsync(productId, quantity, price, daysToExpire);
                return Ok("Restock successful");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("move-to-shelf")]
        public async Task<IActionResult> MoveToShelf(int productId, int quantity, decimal sellPrice)
        {
            try
            {
                await _shelfService.MoveToShelfAsync(productId, quantity, sellPrice);
                return Ok("Move to shelf successful");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("sell")]
        public async Task<IActionResult> Sell(int productId, int quantity, string paymentMethod)
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

using Microsoft.AspNetCore.Mvc;
using StoreManagementSystem.API.Services;
using Microsoft.AspNetCore.Authorization;
using StoreManagementSystem.API.Data.Entities;

namespace StoreManagementSystem.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class SalesController : ControllerBase
    {
        private readonly ISalesService _service;

        public SalesController(ISalesService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Sale>>> GetSales([FromQuery] int? productId = null)
        {
            try
            {
                var sales = await _service.GetAllSalesAsync(productId);
                return Ok(sales);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}

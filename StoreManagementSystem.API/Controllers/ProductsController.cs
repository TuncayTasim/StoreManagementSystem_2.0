using Microsoft.AspNetCore.Mvc;
using StoreManagementSystem.API.Services;
using Microsoft.AspNetCore.Authorization;
using StoreManagementSystem.API.Data.Entities;

namespace StoreManagementSystem.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class ProductsController : ControllerBase
    {
        private readonly IProductService _service;

        public ProductsController(IProductService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Product>>> GetProducts()
        {
            return Ok(await _service.GetAllProductsAsync());
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Product>> GetProduct(int id)
        {
            var product = await _service.GetProductByIdAsync(id);
            if (product == null) return NotFound();
            return Ok(product);
        }

        [HttpPost]
        public async Task<ActionResult<Product>> PostProduct(Product product)
        {
            try
            {
                await _service.CreateProductAsync(product);
                return CreatedAtAction(nameof(GetProduct), new { id = product.Id }, product);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutProduct(int id, Product product)
        {
            try
            {
                if (id != product.Id) return BadRequest("Product ID mismatch");
                await _service.UpdateProductAsync(product);
                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            try
            {
                await _service.DeleteProductAsync(id);
                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}

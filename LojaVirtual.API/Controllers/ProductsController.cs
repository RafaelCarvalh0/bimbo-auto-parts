using LojaVirtual.API.Models.Validators;
using LojaVirtual.Data.Repositories;
using LojaVirtual.Entities.Models.Domain;
using LojaVirtual.Entities.Models.Dto.Request;
using LojaVirtual.Entities.Models.Dto.Response;
using LojaVirtual.Entities.Utils;
using Microsoft.AspNetCore.Mvc;

namespace LojaVirtual.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductsController : ControllerBase
    {
        private readonly ILogger<ProductsController> _logger;
        private readonly IProductRepository _repo;

        public ProductsController(ILogger<ProductsController> logger, IProductRepository repo)
        {
            _logger = logger;
            _repo = repo;
        }

        [HttpGet]
        [Route("[action]")]
        public async Task<ActionResult<IEnumerable<ProductResponse>>> GetAll()
        {
            try
            {
                IEnumerable<Product> products = await _repo.GetAllProductsAsync();
                IEnumerable<ProductResponse> response = products.Select(p => (ProductResponse)p);

                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while getting all products");

                return StatusCode(
                    statusCode: 500,
                    value: new { Title = "An unexpected error occurred.", Message = $"{ex.Message}" }
                );
            }
        }

        [HttpGet]
        [Route("[action]/{id}")]
        public async Task<ActionResult<ProductResponse>> GetById(int id)
        {
            try
            {
                Product product = await _repo.GetProductByIdAsync(id);
                ProductResponse response = product;

                return Ok(response);
            }
            catch (NotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while getting a product by ID");

                return StatusCode(
                   statusCode: 500,
                   value: new { Title = "An unexpected error occurred.", Message = $"{ex.Message}" }
               );
            }
        }

        [HttpPost]
        [Route("[action]")]
        [Validate<ProductCreateRequest>]
        public async Task<IActionResult> Create([FromBody] ProductCreateRequest request)
        {
            try
            {
                await _repo.CreateProductAsync(request);
                return Created(nameof(Create), request);
            }
            catch (NotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while creating a product.");

                return StatusCode(
                    statusCode: 500,
                    value: new { Title = "An unexpected error occurred.", Message = $"{ex.Message}" }
                );
            }
        }

        [HttpPatch]
        [Route("[action]/{id}")]
        [Validate<ProductUpdateRequest>]
        public async Task<IActionResult> Patch([FromRoute] int id, [FromBody] ProductUpdateRequest request)
        {
            try
            {
                await _repo.PatchProductAsync(id, request);
                return Ok("Product updated successfully.");
            }
            catch (NotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while patching a product");

                return StatusCode(
                    statusCode: 500,
                    value: new { Title = "An unexpected error occurred.", Message = $"{ex.Message}" }
                );
            }
        }

        [HttpDelete]
        [Route("[action]/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await _repo.DeleteProductAsync(id);
                return Ok("Product deleted successfully.");
            }
            catch (NotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while deleting a product.");

                return StatusCode(
                    statusCode: 500,
                    value: new { Title = "An unexpected error occurred.", Message = $"{ex.Message}" }
                );
            }
        }
    }
}

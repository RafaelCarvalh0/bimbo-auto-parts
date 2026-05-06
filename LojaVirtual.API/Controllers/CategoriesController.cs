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
    public class CategoriesController : ControllerBase
    {
        private readonly ILogger<CategoriesController> _logger;
        private readonly ICategoryRepository _repo;

        public CategoriesController(ILogger<CategoriesController> logger, ICategoryRepository repo)
        {
            _logger = logger;
            _repo = repo;
        }

        [HttpGet]
        [Route("[action]")]
        public async Task<ActionResult<IEnumerable<CategoryResponse>>> GetAll()
        {
            try
            {
                IEnumerable<Category> categories = await _repo.GetAllCategoriesAsync();
                IEnumerable<CategoryResponse> response = categories.Select(c => (CategoryResponse)c);

                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while getting all categories");

                return StatusCode(
                    statusCode: 500,
                    value: new { Title = "An unexpected error occurred.", Message = $"{ex.Message}" }
                );
            }
        }

        [HttpGet]
        [Route("[action]/{id}")]
        public async Task<ActionResult<CategoryResponse>> GetById(int id)
        {
            try
            {
                Category category = await _repo.GetCategoryByIdAsync(id);
                CategoryResponse response = category;

                return Ok(response);
            }
            catch (NotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while getting a category by ID");

                return StatusCode(
                   statusCode: 500,
                   value: new { Title = "An unexpected error occurred.", Message = $"{ex.Message}" }
               );
            }
        }

        [HttpPost]
        [Route("[action]")]
        [Validate<CategoryCreateRequest>]
        public async Task<IActionResult> Create([FromBody] CategoryCreateRequest request)
        {
            try
            {
                await _repo.CreateCategoryAsync(request);
                return Created(nameof(Create), request);
            }
            catch (ConflictException ex)
            {
                return Conflict(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while creating a category.");

                return StatusCode(
                    statusCode: 500,
                    value: new { Title = "An unexpected error occurred.", Message = $"{ex.Message}" }
                );
            }
        }

        [HttpPatch]
        [Route("[action]/{id}")]
        [Validate<CategoryUpdateRequest>]
        public async Task<IActionResult> Patch([FromRoute] int id, [FromBody] CategoryUpdateRequest request)
        {
            try
            {
                await _repo.PatchCategoryAsync(id, request);
                return Ok("Category updated successfully.");
            }
            catch (ConflictException ex)
            {
                return Conflict(new { message = ex.Message });
            }
            catch (NotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while patching a category");

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
                await _repo.DeleteCategoryAsync(id);
                return Ok("Category deleted successfully.");
            }
            catch (NotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while deleting a category.");

                return StatusCode(
                    statusCode: 500,
                    value: new { Title = "An unexpected error occurred.", Message = $"{ex.Message}" }
                );
            }
        }
    }
}

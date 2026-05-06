using LojaVirtual.Entities.Models.Dto.Response;
using LojaVirtual.Services.Application;
using LojaVirtual.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace LojaVirtual.Controllers
{
    public class CategoriesController : Controller
    {
        private readonly IApplicationFactory _applicationFactory;
        private readonly ILogger<CategoriesController> _logger;

        public CategoriesController(ILogger<CategoriesController> logger, IApplicationFactory aplicationFactory)
        {
            _applicationFactory = aplicationFactory;
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public async Task<JsonResult> GetTable()
        {
            try
            {
                string response = await _applicationFactory.CallWebService("api/Categories/GetAll", RequestTypeEnum.GET);

                List<CategoryResponse>? model = JsonSerializer.Deserialize<List<CategoryResponse>>(response, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

                return Json(new { data = model });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while loading categories");
                return Json(new { error = ex.Message });
            }
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CategoryViewModel request)
        {
            try
            {
                await _applicationFactory.CallWebService("api/Categories/Create", RequestTypeEnum.POST, request);

                return Ok(new { message = "Category created successfully." });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while creating category");
                return StatusCode(500, new
                {
                    message = Helper.ParseErrorMessage(ex.Message)
                });
            }
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            try
            {
                string response = await _applicationFactory.CallWebService($"api/Categories/GetById/{id}", RequestTypeEnum.GET);

                CategoryResponse? model = JsonSerializer.Deserialize<CategoryResponse>(response, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

                CategoryViewModel viewModel = model!;
                return View(viewModel);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while loading category {Id}", id);
                return RedirectToAction("Index");
            }
        }

        [HttpPost]
        public async Task<IActionResult> Edit([FromBody] CategoryViewModel request)
        {
            try
            {
                await _applicationFactory.CallWebService($"api/Categories/Patch/{request.Id}", RequestTypeEnum.PATCH, request);
                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while updating category {Id}", request.Id);
                return BadRequest(new { message = Helper.ParseErrorMessage(ex.Message) });
            }
        } 

        [HttpDelete]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await _applicationFactory.CallWebService($"api/Categories/Delete/{id}", RequestTypeEnum.DELETE);

                return Ok(new { message = "Category deleted successfully." });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while deleting category {Id}", id);

                return StatusCode(500, new
                {
                    message = Helper.ParseErrorMessage(ex.Message)
                });
            }
        }
    }
}

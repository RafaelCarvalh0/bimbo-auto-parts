using LojaVirtual.Entities.Models.Dto.Response;
using LojaVirtual.Services.Application;
using LojaVirtual.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace LojaVirtual.Controllers
{
    public class ProductsController : Controller
    {
        private readonly IApplicationFactory _applicationFactory;
        private readonly ILogger<ProductsController> _logger;

        public ProductsController(ILogger<ProductsController> logger, IApplicationFactory aplicationFactory)
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
                string response = await _applicationFactory.CallWebService("api/Products/GetAll", RequestTypeEnum.GET);

                List<ProductResponse>? model = JsonSerializer.Deserialize<List<ProductResponse>>(response, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

                return Json(new { data = model });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while loading products");
                return Json(new { error = ex.Message });
            }
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] ProductViewModel request)
        {
            try
            {
                await _applicationFactory.CallWebService("api/Products/Create", RequestTypeEnum.POST, request);

                return Ok(new { message = "Product created successfully." });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while creating product");
                return StatusCode(500, new
                {
                    message = ex.Message
                });
            }
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            try
            {
                string response = await _applicationFactory.CallWebService($"api/Products/GetById/{id}", RequestTypeEnum.GET);

                ProductResponse? model = JsonSerializer.Deserialize<ProductResponse>(response, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

                ProductViewModel? viewModel = model!;

                return View(viewModel);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while loading product {Id}", id);
                return RedirectToAction("Index");
            }
        }

        [HttpPost]
        public async Task<IActionResult> Edit(ProductViewModel request)
        {
            try
            {
                await _applicationFactory.CallWebService($"api/Products/Patch/{request.Id}", RequestTypeEnum.PATCH, request);

                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while updating product {Id}", request.Id);

                ModelState.AddModelError(string.Empty, ex.Message);
                return View(request);
            }
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await _applicationFactory.CallWebService($"api/Products/Delete/{id}", RequestTypeEnum.DELETE);

                return Ok(new { message = "Product deleted successfully." });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while deleting product {Id}", id);

                return StatusCode(500, new
                {
                    message = ex.Message
                });
            }
        }
    }
}

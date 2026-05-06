using LojaVirtual.Data.DatabaseContext;
using LojaVirtual.Entities.Models.Domain;
using LojaVirtual.Entities.Models.Dto.Request;
using LojaVirtual.Entities.Utils;
using Microsoft.EntityFrameworkCore;

namespace LojaVirtual.Data.Repositories
{
    public interface IProductRepository
    {
        Task<IEnumerable<Product>> GetAllProductsAsync();
        Task<Product> GetProductByIdAsync(int id);
        Task CreateProductAsync(ProductCreateRequest request);
        Task PatchProductAsync(int id, ProductUpdateRequest request);
        Task DeleteProductAsync(int id);
    }

    public class ProductRepository(AppDbContext context) : IProductRepository
    {
        public async Task<IEnumerable<Product>> GetAllProductsAsync()
        {
            return await context.Products.AsNoTracking().Include(p => p.Category).ToListAsync();
        }

        public async Task<Product> GetProductByIdAsync(int id)
        {
            return await context.Products.AsNoTracking().Include(p => p.Category).FirstOrDefaultAsync(p => p.Id == id) ?? throw new NotFoundException("Product not found.");
        }

        public async Task CreateProductAsync(ProductCreateRequest request)
        {
            _ = await context.Categories.FindAsync(request.CategoryId) ?? throw new NotFoundException("Category not found.");

            var product = new Product
            {
                Name = request.Name,
                Description = request.Description,
                Price = request.Price,
                CategoryId = request.CategoryId
            };

            context.Products.Add(product);
            await context.SaveChangesAsync();
        }

        public async Task PatchProductAsync(int id, ProductUpdateRequest request)
        {
            Product existing = await context.Products.FindAsync(id) ?? throw new NotFoundException("Product not found.");

            if (!string.IsNullOrWhiteSpace(request.Name))
                existing.Name = request.Name;

            if (!string.IsNullOrWhiteSpace(request.Description))
                existing.Description = request.Description;

            if (request.Price > 0)
                existing.Price = request.Price.Value;

            if (request.CategoryId > 0)
            {
                _ = await context.Categories.FindAsync(request.CategoryId) ?? throw new NotFoundException("Category not found.");
                existing.CategoryId = request.CategoryId.Value;
            }

            await context.SaveChangesAsync();
        }

        public async Task DeleteProductAsync(int id)
        {
            var product = await context.Products.FindAsync(id) ?? throw new NotFoundException("Product not found.");

            context.Products.Remove(product);
            await context.SaveChangesAsync();
        }
    }
}

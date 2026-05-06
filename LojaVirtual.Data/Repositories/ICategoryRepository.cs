using LojaVirtual.Data.DatabaseContext;
using LojaVirtual.Entities.Models.Domain;
using LojaVirtual.Entities.Models.Dto.Request;
using LojaVirtual.Entities.Utils;
using Microsoft.EntityFrameworkCore;

namespace LojaVirtual.Data.Repositories
{
    public interface ICategoryRepository
    {
        Task<IEnumerable<Category>> GetAllCategoriesAsync();
        Task<Category> GetCategoryByIdAsync(int id);
        Task CreateCategoryAsync(CategoryCreateRequest request);
        Task PatchCategoryAsync(int id, CategoryUpdateRequest request);
        Task DeleteCategoryAsync(int id);
    }

    public class CategoryRepository(AppDbContext context) : ICategoryRepository
    {
        public async Task<IEnumerable<Category>> GetAllCategoriesAsync()
        {
            return await context.Categories.AsNoTracking().ToListAsync();
        }

        public async Task<Category> GetCategoryByIdAsync(int id)
        {
            return await context.Categories.AsNoTracking().FirstOrDefaultAsync(c => c.Id == id) ?? throw new NotFoundException($"Category with ID {id} not found.");
        }

        public async Task CreateCategoryAsync(CategoryCreateRequest request)
        {
            bool exists = await context.Categories.AnyAsync(c => c.Name.Trim().ToLower() == request.Name.Trim().ToLower());

            if (exists)
                throw new ConflictException("Category already exists");

            var category = new Category
            {
                Name = request.Name,
                Description = request.Description
            };

            await context.Categories.AddAsync(category);
            await context.SaveChangesAsync();
        }

        public async Task PatchCategoryAsync(int id, CategoryUpdateRequest request)
        {
            if (!string.IsNullOrWhiteSpace(request.Name))
            {
                bool existingCategory = await context.Categories.AnyAsync(c => c.Name.Trim().ToLower() == request.Name.Trim().ToLower() && c.Id != id);

                if (existingCategory)
                    throw new ConflictException("Category with this name already exists.");
            }

            Category existing = await context.Categories.FindAsync(id) ?? throw new NotFoundException($"Category with ID {id} not found.");

            if (!string.IsNullOrWhiteSpace(request.Name))
                existing.Name = request.Name;

            if (!string.IsNullOrWhiteSpace(request.Description))
                existing.Description = request.Description;

            await context.SaveChangesAsync();
        }

        public async Task DeleteCategoryAsync(int id)
        {
            Category existing = await context.Categories.FindAsync(id) ?? throw new NotFoundException($"Category with ID {id} not found.");

            List<Product> productsInCategory = await context.Products.Where(p => p.CategoryId == id).ToListAsync();

            if (productsInCategory.Any())
                throw new Exception("Cannot delete category because it has associated products.");

            context.Categories.Remove(existing);
            await context.SaveChangesAsync();
        }
    }
}

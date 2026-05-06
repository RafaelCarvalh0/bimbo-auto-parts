using LojaVirtual.Entities.Models.Dto.Response;

namespace LojaVirtual.ViewModels
{
    public class CategoryViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; }

        public static implicit operator CategoryViewModel(CategoryResponse category)
            => new CategoryViewModel
            {
                Id = category.Id,
                Name = category.Name,
                Description = category.Description
            };
    }
}

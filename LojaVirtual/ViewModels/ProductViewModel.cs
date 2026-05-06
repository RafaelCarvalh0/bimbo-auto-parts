using LojaVirtual.Entities.Models.Domain;
using LojaVirtual.Entities.Models.Dto.Response;

namespace LojaVirtual.ViewModels
{
    public class ProductViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; }
        public decimal Price { get; set; }
        public int CategoryId { get; set; }
        public string CategoryName { get; set; }

        public static implicit operator ProductViewModel(ProductResponse product)
                => new ProductViewModel
                {
                    Id = product.Id,
                    Name = product.Name,
                    Description = product.Description,
                    Price = product.Price,
                    CategoryId = product.CategoryId,
                    CategoryName = product.CategoryName
                };
    }
}

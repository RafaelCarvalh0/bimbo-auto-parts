namespace LojaVirtual.Entities.Models.Dto.Response
{
    public record ProductResponse(int Id, string Name, string Description, decimal Price, int CategoryId, string CategoryName)
    {
        public static implicit operator ProductResponse(Domain.Product product)
            => new(product.Id, product.Name, product.Description, product.Price, product.CategoryId, product.Category.Name);
    }
}

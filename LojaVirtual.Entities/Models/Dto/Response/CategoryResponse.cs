namespace LojaVirtual.Entities.Models.Dto.Response
{
    public record CategoryResponse(int Id, string Name, string Description)
    {
        public static implicit operator CategoryResponse(Domain.Category category)
            => new(category.Id, category.Name, category.Description);
    }
}
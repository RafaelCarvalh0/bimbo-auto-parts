namespace LojaVirtual.Entities.Models.Dto.Request
{
    public record ProductCreateRequest(string Name, string Description, decimal Price, int CategoryId);
}

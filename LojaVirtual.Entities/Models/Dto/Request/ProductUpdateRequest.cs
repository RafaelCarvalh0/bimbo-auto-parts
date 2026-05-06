namespace LojaVirtual.Entities.Models.Dto.Request
{
    public record ProductUpdateRequest(string? Name, string? Description, decimal? Price, int? CategoryId);
}

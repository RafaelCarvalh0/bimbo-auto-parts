using System.Text.Json.Serialization;

namespace LojaVirtual.Models
{
    public class ValidationError
    {
        [JsonPropertyName("propertyName")]
        public string PropertyName { get; set; } = string.Empty;

        [JsonPropertyName("errorMessage")]
        public string ErrorMessage { get; set; } = string.Empty;
    }
}

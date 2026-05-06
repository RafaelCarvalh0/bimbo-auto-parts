using LojaVirtual.Models;
using System.Text.Json;

namespace LojaVirtual
{
    public static class Helper
    {
        public static string ParseErrorMessage(string rawMessage)
        {
            try
            {
                var validationErrors = JsonSerializer.Deserialize<List<ValidationError>>(rawMessage);
                var first = validationErrors?.FirstOrDefault();
                if (first != null) return first.ErrorMessage;
            }
            catch { }

            try
            {
                var simple = JsonSerializer.Deserialize<Dictionary<string, string>>(rawMessage);
                if (simple != null && simple.ContainsKey("message"))
                    return simple["message"];
            }
            catch { }

            return rawMessage;
        }
    }
}

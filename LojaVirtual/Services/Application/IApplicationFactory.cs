using System.Net;
using System.Text;
using System.Text.Encodings.Web;
using System.Text.Json;

namespace LojaVirtual.Services.Application
{
    public interface IApplicationFactory
    {
        Task<string> CallWebService(string endPoint, RequestTypeEnum requestTypeEnum, object request = null);
    }

    public class ApplicationFactory : IApplicationFactory
    {
        private readonly HttpClient _httpClient;
        public ApplicationFactory(IHttpClientFactory factory, IConfiguration configuration)
        {
            _httpClient = factory.CreateClient();
            _httpClient.BaseAddress = new Uri(configuration["Api:UrlBase"]);
        }

        public async Task<string> CallWebService(string endPoint, RequestTypeEnum requestTypeEnum, object request = null)
        {
            try
            {
                HttpRequestMessage requestMessage = requestTypeEnum switch
                {
                    RequestTypeEnum.GET => new HttpRequestMessage(HttpMethod.Get, endPoint),
                    RequestTypeEnum.POST => new HttpRequestMessage(HttpMethod.Post, endPoint),
                    RequestTypeEnum.PUT => new HttpRequestMessage(HttpMethod.Put, endPoint),
                    RequestTypeEnum.PATCH => new HttpRequestMessage(HttpMethod.Patch, endPoint),
                    RequestTypeEnum.DELETE => new HttpRequestMessage(HttpMethod.Delete, endPoint    ),
                    _ => throw new NotImplementedException()
                };

                if (request is not null)
                {
                    var json = System.Text.Json.JsonSerializer.Serialize(request, new JsonSerializerOptions { Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping, PropertyNamingPolicy = JsonNamingPolicy.CamelCase });
                    requestMessage.Content = new StringContent(json, Encoding.UTF8, "application/json");
                }

                HttpResponseMessage response = await _httpClient.SendAsync(requestMessage);

                var content = await response.Content.ReadAsStringAsync();

                switch (Convert.ToInt32(response.StatusCode))
                {
                    case >= 200 and <= 299:
                        return string.IsNullOrWhiteSpace(content) ? null : content;

                    case 400 or 404 or 409:
                        throw new Exception(!string.IsNullOrWhiteSpace(content) ? content : $"API Error: {response.StatusCode}");

                    case 500:
                        throw new Exception(JsonDocument.Parse(content.ToLower()).RootElement.TryGetProperty("message", out JsonElement msg500) ? msg500.ToString() ?? throw new Exception($"{content}") : throw new Exception($"{content}"));

                    default:
                        throw new Exception($"Unexpected API response: {response.StatusCode}");
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}

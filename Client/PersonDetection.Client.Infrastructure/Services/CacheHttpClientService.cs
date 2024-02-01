using System.Text;
using System.Text.Json;
using PersonDetection.Client.Application.Models.Types;
using PersonDetection.Client.Infrastructure.Common;

namespace PersonDetection.Client.Infrastructure.Services;

public class CacheHttpClientService(HttpClientProvider httpClientProvider)
{
    private readonly HttpClient _httpClient = httpClientProvider.CreateClient();
    private static readonly JsonSerializerOptions JsonSerializerOptions = new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase
    };
    
    public async Task<Result<T, Error>> PostAsync<T>(string url, T data, CancellationToken cancellationToken = default)
    {
        try
        {
            var json = JsonSerializer.Serialize(data, JsonSerializerOptions);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync(url, content, cancellationToken);
            
            if (!response.IsSuccessStatusCode)
            {
                return new Error(response.ReasonPhrase ?? InfrastructureErrorMessages.UnknownError);
            }

            var jsonResult = await response.Content.ReadAsStringAsync(cancellationToken);

            return JsonSerializer.Deserialize<T>(jsonResult, JsonSerializerOptions)!;
        }
        catch (Exception e)
        {
            // There is a possibility that the HTTP client throws a native exception.
            return new Error(e.Message);
        }
    }
}
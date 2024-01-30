using System.Text;
using System.Text.Json;
using Microsoft.Extensions.Caching.Memory;
using PersonDetection.Client.Application.Models.Types;
using PersonDetection.Client.Infrastructure.Common;
using PersonDetection.Client.Infrastructure.Common.Extensions;

namespace PersonDetection.Client.Infrastructure.Services;

public class CacheHttpClientService(
    HttpClientProvider httpClientProvider,
    IMemoryCache memoryCache,
    IConnectivity connectivity,
    IInfrastructureConfiguration configuration)
{
    private readonly HttpClient _httpClient = httpClientProvider.CreateClient();
    private static readonly JsonSerializerOptions _jsonSerializerOptions = new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase
    };

    public async Task<T?> GetAsync<T>(string url, CancellationToken cancellationToken = default)
    {
        var json = await GetOrCreateJsonAsync(url, cancellationToken);
        var result = JsonSerializer.Deserialize<T>(json);
        
        return result ?? default;
    }
    
    public async Task<Result<T, Error>> PostAsync<T>(string url, T data, CancellationToken cancellationToken = default)
    {
        try
        {
            var json = JsonSerializer.Serialize(data, _jsonSerializerOptions);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync(url, content, cancellationToken);
            
            if (!response.IsSuccessStatusCode)
            {
                return new Error(response.ReasonPhrase ?? "Unknown error");
            }

            var jsonResult = await response.Content.ReadAsStringAsync(cancellationToken);

            return JsonSerializer.Deserialize<T>(jsonResult, _jsonSerializerOptions)!;
        }
        catch (Exception e)
        {
            // There are change, what the http client throws native exception.
            return new Error(e.Message);
        }
    }

    private async Task<Result<string, Error>> GetOrCreateJsonAsync(string url, CancellationToken cancellationToken = default)
    {
        var cacheKey = url.RemoveSpecialCharacters();
        
        if (memoryCache.TryGetValue(cacheKey, out string? json))
        {
            return json!;
        }

        if (connectivity.NetworkAccess != NetworkAccess.Internet)
        {
            return new Error("No internet connection");
        }
        
        var response = await _httpClient.GetAsync(url, cancellationToken);
        
        if (!response.IsSuccessStatusCode)
        {
            return new Error(response.ReasonPhrase ?? "Unknown error");
        }

        json = await response.Content.ReadAsStringAsync(cancellationToken);
        memoryCache.Set(cacheKey, json, configuration.CacheExpirationTime);
        
        return json;
    }
}
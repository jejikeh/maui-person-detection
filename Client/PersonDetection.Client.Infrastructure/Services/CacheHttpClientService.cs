using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using Microsoft.Extensions.Caching.Memory;
using PersonDetection.Client.Infrastructure.Common;
using PersonDetection.Client.Infrastructure.Common.Exceptions;
using PersonDetection.Client.Infrastructure.Common.Extensions;

namespace PersonDetection.Client.Infrastructure.Services;

public class CacheHttpClientService(
    HttpClientProvider httpClientProvider,
    IMemoryCache memoryCache,
    IConnectivity connectivity,
    IInfrastructureConfiguration configuration)
{
    private readonly HttpClient _httpClient = httpClientProvider.CreateClient();

    public async Task<T?> GetAsync<T>(string url, CancellationToken cancellationToken = default)
    {
        var json = await GetOrCreateJsonAsync(url, cancellationToken);
        var result = JsonSerializer.Deserialize<T>(json);
        
        return result ?? default;
    }
    
    public async Task<T> PostAsync<T>(string url, T data, CancellationToken cancellationToken = default)
    {
        var json = JsonSerializer.Serialize(data);
        var content = new StringContent(json, Encoding.UTF8, "application/json");

        var response = await _httpClient.PostAsync(url, content, cancellationToken);
        response.EnsureSuccessStatusCode();
        
        var jsonResult = await response.Content.ReadAsStringAsync(cancellationToken);
        
        return JsonSerializer.Deserialize<T>(jsonResult)!;
    }

    private async Task<string> GetOrCreateJsonAsync(string url, CancellationToken cancellationToken = default)
    {
        var cacheKey = url.RemoveSpecialCharacters();
        if (memoryCache.TryGetValue(cacheKey, out string? json))
        {
            return json!;
        }

        if (connectivity.NetworkAccess != NetworkAccess.Internet)
        {
            throw new InternetConnectionException();
        }
        
        var response = await _httpClient.GetAsync(url, cancellationToken);
        response.EnsureSuccessStatusCode();
        
        json = await response.Content.ReadAsStringAsync(cancellationToken);
        memoryCache.Set(cacheKey, json, configuration.CacheExpirationTime);
        
        return json;
    }
}
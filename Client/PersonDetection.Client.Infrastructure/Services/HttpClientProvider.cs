using System.Net.Http.Headers;

namespace PersonDetection.Client.Infrastructure.Services;

public class HttpClientProvider(IHttpClientFactory _httpClientFactory)
{
    public HttpClient CreateClient()
    {
        var httpClient = _httpClientFactory.CreateClient();
        httpClient.DefaultRequestHeaders.Accept.Clear();
        httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        
        return httpClient;
    }
}
using System.Net.Http.Headers;

namespace PersonDetection.Client.Infrastructure.Services;

public class HttpClientProvider(IHttpClientFactory httpClientFactory)
{
    public HttpClient CreateClient()
    {
        var httpClient = httpClientFactory.CreateClient();

        httpClient.DefaultRequestHeaders.Accept.Clear();
        httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        
        return httpClient;
    }
}
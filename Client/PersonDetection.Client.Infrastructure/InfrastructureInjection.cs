using PersonDetection.Client.Application.Services;
using PersonDetection.Client.Infrastructure.Services;

namespace PersonDetection.Client.Infrastructure;

public static class InfrastructureInjection
{
    public static IServiceCollection UseInfrastructure(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddHttpClient();
        serviceCollection.AddSingleton<HttpClientProvider>();
        serviceCollection.AddMemoryCache();
        serviceCollection.AddScoped<CacheHttpClientService>();
        serviceCollection.AddScoped<IPhotoProcessService, HttpPhotoProcessService>();
        serviceCollection.AddSingleton<IProcessedPhotoGallery, ProcessedPhotoGallery>();
        
        return serviceCollection;
    }
}
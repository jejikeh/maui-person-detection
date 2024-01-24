using PersonDetection.Client.Application.Services;
using PersonDetection.Client.Infrastructure.Services;

namespace PersonDetection.Client.Infrastructure;

public static class InfrastructureInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddSingleton<HttpClientProvider>();
        serviceCollection.AddScoped<CacheHttpClientService>();
        serviceCollection.AddScoped<IPhotoProcessService, HttpPhotoProcessService>();
        serviceCollection.AddSingleton<IProcessedPhotoGallery, ProcessedPhotoGallery>();
        
        return serviceCollection;
    }
}
using PersonDetection.Client.Application.Services;
using PersonDetection.Client.Infrastructure.Services;

namespace PersonDetection.Client.Infrastructure;

public static class InfrastructureInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddSingleton<IPhotoGallery, PhotoGallery>();
        serviceCollection.AddSingleton<PhotoSaverService>();
        serviceCollection.AddHttpClientProviders();
        
        return serviceCollection;
    }

    private static IServiceCollection AddHttpClientProviders(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddSingleton<HttpClientProvider>();
        serviceCollection.AddScoped<CacheHttpClientService>();
        serviceCollection.AddScoped<IPhotoProcessService, HttpPhotoProcessService>();
        
        return serviceCollection;
    }
}
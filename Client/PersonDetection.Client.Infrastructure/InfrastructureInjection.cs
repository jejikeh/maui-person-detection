using PersonDetection.Client.Application.Services;
using PersonDetection.Client.Infrastructure.Common;
using PersonDetection.Client.Infrastructure.Services;
using PersonDetection.ImageProcessing;

namespace PersonDetection.Client.Infrastructure;

public static class InfrastructureInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddSingleton<IPhotoGallery, PhotoGallery>();
        serviceCollection.AddSingleton<PhotoSaverService>();
        
        return serviceCollection;
    }

    public static IServiceCollection AddHttpClientProviders(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddSingleton<HttpClientProvider>();
        serviceCollection.AddScoped<CacheHttpClientService>();
        serviceCollection.AddScoped<IPhotoProcessService, HttpPhotoProcessService>();
        
        return serviceCollection;
    }

    public static IServiceCollection AddYoloServices(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddSingleton<YoloImageProcessing>();
        serviceCollection.AddSingleton<IPhotoProcessService, YoloPhotoProcessService>();
        
        return serviceCollection;
    }
}
using Microsoft.Extensions.Configuration;
using PersonDetection.Client.Application.Services;
using PersonDetection.Client.Infrastructure.Common.Options;
using PersonDetection.Client.Infrastructure.Services;

namespace PersonDetection.Client.Infrastructure;

public static class InfrastructureInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection serviceCollection, IConfiguration configuration)
    {
        return serviceCollection
            .AddSingleton<IPhotoGallery, PhotoGallery>()
            .AddSingleton<PhotoSaverService>()
            .AddInfrastructureOptions(configuration);
    }

    public static IServiceCollection AddHttpClientProviders(this IServiceCollection serviceCollection)
    {
        return serviceCollection
            .AddSingleton<HttpClientProvider>()
            .AddScoped<CacheHttpClientService>()
            .AddScoped<IPhotoProcessService, HttpPhotoProcessService>();
    }

    private static IServiceCollection AddInfrastructureOptions(this IServiceCollection serviceCollection, IConfiguration configuration)
    {
        return serviceCollection
            .ConfigureOptions<PhotoGalleryOptions>(configuration)
            .ConfigureOptions<PhotoSaverOptions>(configuration)
            .ConfigureOptions<HttpPhotoProcessOptions>(configuration);
    }

    private static IServiceCollection ConfigureOptions<T>(this IServiceCollection serviceCollection, IConfiguration configuration) where T : class
    {
        return serviceCollection.Configure<T>(configuration.GetSection(typeof(T).Name));
    }
}
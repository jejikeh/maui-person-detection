using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PersonDetection.ImageProcessing.Options;

namespace PersonDetection.ImageProcessing;

public static class ImageProcessingInjection
{
    public static IServiceCollection AddYoloImageProcessing(this IServiceCollection serviceCollection, IConfiguration configuration)
    {
        return serviceCollection
            .ConfigureOptions<YoloImageProcessingOptions>(configuration)
            .AddSingleton<YoloImageProcessing>();
    }
    
    private static IServiceCollection ConfigureOptions<T>(this IServiceCollection serviceCollection, IConfiguration configuration) where T : class
    {
        return serviceCollection.Configure<T>(options =>
        {
            configuration
                .GetSection(typeof(T).Name)
                .Bind(options);
        });
    }
}
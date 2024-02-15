using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace PersonDetection.ImageSegmentation.ModelConverter;

public static class ImageSegmentationInjection
{
    public static IServiceCollection AddYoloImageSegmentationProcessing(this IServiceCollection serviceCollection, IConfiguration configuration)
    {
        return serviceCollection
            .ConfigureOptions<ModelLoaderOptions>(configuration)
            .AddSingleton<YoloImageSegmentation>();
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
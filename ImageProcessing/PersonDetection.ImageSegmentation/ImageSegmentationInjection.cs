using Microsoft.Extensions.DependencyInjection;

namespace PersonDetection.ImageSegmentation.ModelConverter;

public static class ImageSegmentationInjection
{
    public static IServiceCollection AddYoloImageSegmentationProcessing(this IServiceCollection serviceCollection)
    {
        return serviceCollection.AddSingleton<YoloImageSegmentation>();
    }
}
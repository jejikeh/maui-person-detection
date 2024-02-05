using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PersonDetection.Backend.Application.Services;
using PersonDetection.ImageProcessing;

namespace PersonDetection.Backend.Application;

public static class ApplicationInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection serviceCollection, IConfiguration configuration)
    {
        return serviceCollection
            .AddYoloImageProcessing(configuration)
            .AddSingleton<PhotoProcessingService>();
    }
}
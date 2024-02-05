using Microsoft.Extensions.DependencyInjection;
using PersonDetection.Backend.Infrastructure.Services;
using PersonDetection.ImageProcessing;
using PersonDetection.ImageProcessing.Services;

namespace PersonDetection.Backend.Infrastructure;

public static class InfrastructureInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection serviceCollection)
    {
        return serviceCollection.AddSingleton<IFileSystemStreamProvider, CoreFileSystemStreamProvider>();
    }
}
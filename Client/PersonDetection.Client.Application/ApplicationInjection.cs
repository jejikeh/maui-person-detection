using Microsoft.Extensions.DependencyInjection;
using PersonDetection.Client.Application.Services;
using PersonDetection.Client.Application.Services.Implementations;

namespace PersonDetection.Client.Application;

public static class ApplicationInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddTransient<PhotoService>();
        
        return serviceCollection;
    }
}


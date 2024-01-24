using PersonDetection.Client.Application;
using PersonDetection.Client.Infrastructure;
using PersonDetection.Client.Infrastructure.Common;
using PersonDetection.Client.Pages;

namespace PersonDetection.Client.Configuration;

public static class InjectionsConfiguration
{
    public static IServiceCollection AddInjections(this IServiceCollection serviceCollection)
    {
        return serviceCollection
            .AddConfiguration()
            .AddDeviceAccessServices()
            .AddPlatformServiceImplementations()
            .AddPages()
            .AddApplication()
            .AddInfrastructure();
    }

    private static IServiceCollection AddConfiguration(this IServiceCollection serviceCollection)
    {
        var clientConfiguration = new ClientConfiguration();
        serviceCollection.AddSingleton(clientConfiguration);
        serviceCollection.AddSingleton<IInfrastructureConfiguration>(clientConfiguration);

        return serviceCollection;
    }

    private static IServiceCollection AddDeviceAccessServices(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddSingleton(Connectivity.Current);
        serviceCollection.AddHttpClient();
        serviceCollection.AddMemoryCache();
        
        return serviceCollection;
    }

    private static IServiceCollection AddPages(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddTransient<ChoosePhotoPage>();
        serviceCollection.AddTransient<StreamVideoPage>();
        
        return serviceCollection;
    }
}
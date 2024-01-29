using CommunityToolkit.Maui.Storage;
using PersonDetection.Client.Application;
using PersonDetection.Client.Infrastructure;
using PersonDetection.Client.Infrastructure.Common;
using PersonDetection.Client.Pages;
using PersonDetection.Client.Services;
using PersonDetection.Client.ViewModels;
using PersonDetection.ImageProcessing.Configuration;

namespace PersonDetection.Client.Configuration;

public static class InjectionsConfiguration
{
    public static IServiceCollection AddInjections(this IServiceCollection serviceCollection)
    {
        var configuration = serviceCollection.AddConfiguration();

        serviceCollection
            .AddDeviceAccessServices()
            .AddPlatformServiceImplementations()
            .AddServices()
            .AddPages()
            .AddViewModels()
            .AddApplication()
            .AddInfrastructure()
            .AddPhotoProcessServices(configuration.PhotoProcessProvider);

        return serviceCollection;
    }

    private static IServiceCollection AddServices(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddSingleton<IExceptionHandler, ExceptionUiDisplayService>();
        
        return serviceCollection;
    }

    private static ClientConfiguration AddConfiguration(this IServiceCollection serviceCollection)
    {
        var clientConfiguration = new ClientConfiguration();
        serviceCollection.AddSingleton(clientConfiguration);
        serviceCollection.AddSingleton<IInfrastructureConfiguration>(clientConfiguration);
        serviceCollection.AddSingleton<IImageProcessingConfiguration>(clientConfiguration);
        
        return clientConfiguration;
    }

    private static IServiceCollection AddDeviceAccessServices(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddSingleton(Connectivity.Current);
        serviceCollection.AddHttpClient();
        serviceCollection.AddMemoryCache();
        serviceCollection.AddSingleton(FileSaver.Default);
        
        return serviceCollection;
    }

    private static IServiceCollection AddPages(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddTransient<ChoosePhotoPage>();
        serviceCollection.AddTransient<PhotoPage>();
        serviceCollection.AddTransient<StreamCameraPage>();
        
        return serviceCollection;
    }
    
    private static IServiceCollection AddViewModels(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddTransient<ChoosePhotoViewModel>();
        serviceCollection.AddTransient<PhotoViewModel>();
        serviceCollection.AddTransient<StreamCameraViewModel>();
        
        return serviceCollection;
    }
}
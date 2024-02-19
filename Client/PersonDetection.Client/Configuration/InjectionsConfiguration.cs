using CommunityToolkit.Maui.Storage;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using PersonDetection.Client.Application;
using PersonDetection.Client.Common.Options;
using PersonDetection.Client.Infrastructure;
using PersonDetection.Client.Pages;
using PersonDetection.Client.Services;
using PersonDetection.Client.ViewModels;

namespace PersonDetection.Client.Configuration;

public static class InjectionsConfiguration
{
    public static IServiceCollection AddInjections(this IServiceCollection serviceCollection, IConfiguration configuration)
    {
        var clientOptions = new ClientOptions();
        configuration
            .GetSection(nameof(ClientOptions))
            .Bind(clientOptions);

        serviceCollection.AddSingleton(Options.Create(clientOptions));
    
        serviceCollection
            .AddDeviceAccessServices()
            .AddPlatformServiceImplementations()
            .AddServices()
            .AddPages()
            .AddViewModels()
            .AddApplication()
            .AddInfrastructure(configuration)
            .AddPhotoProcessServices(configuration, clientOptions.PhotoProcessProvider);

        return serviceCollection;
    }

    private static IServiceCollection AddServices(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddSingleton<IExceptionHandler, ExceptionUiDisplayService>();
        
        return serviceCollection;
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
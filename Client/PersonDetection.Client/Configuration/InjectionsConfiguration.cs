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
    public static IServiceCollection AddInjections(this MauiAppBuilder builder, IConfiguration configuration)
    {
        var clientOptions = new ClientOptions();
        
        configuration
            .GetSection(nameof(ClientOptions))
            .Bind(clientOptions);

        builder.Services.AddSingleton(Options.Create(clientOptions));
    
        builder
            .AddPlatformServiceImplementations()
            .AddDeviceAccessServices()
            .AddServices()
            .AddPages()
            .AddViewModels()
            .AddApplication()
            .AddInfrastructure(configuration)
            .AddPhotoProcessServices(configuration, clientOptions.PhotoProcessProvider);

        return builder.Services;
    }

    private static IServiceCollection AddServices(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddSingleton<IExceptionHandler, ExceptionUiDisplayService>();
        
        return serviceCollection;
    }
    
    private static IServiceCollection AddDeviceAccessServices(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddSingleton(Connectivity.Current);
        serviceCollection.AddSingleton(FileSaver.Default);
        serviceCollection.AddHttpClient();
        serviceCollection.AddMemoryCache();
        
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
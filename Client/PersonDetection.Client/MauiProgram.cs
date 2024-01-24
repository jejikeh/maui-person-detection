using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Maui.Controls.Hosting;
using Microsoft.Maui.Hosting;
using Microsoft.Maui.Networking;
using PersonDetection.Client.Configuration;
using PersonDetection.Client.Application;
using PersonDetection.Client.Application.Services;
using PersonDetection.Client.Infrastructure;
using PersonDetection.Client.Infrastructure.Common;
using PersonDetection.Client.Pages;

namespace PersonDetection.Client;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();
        builder
            .UseMauiApp<App>()
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
            });
        
        var clientConfiguration = new ClientConfiguration();
        builder.Services.AddSingleton(clientConfiguration);
        builder.Services.AddSingleton<IInfrastructureConfiguration>(clientConfiguration);

        var connectivity = Connectivity.Current;
        builder.Services.AddSingleton(connectivity);
        
        
        builder.Services.AddTransient<ChoosePhotoPage>();
        builder.Services.AddSingleton<StreamVideoPage>();

        builder.Services.UseApplication();
        builder.Services.UseInfrastructure();

#if DEBUG
        builder.Logging.AddDebug();
#endif

        return builder.Build();
    }
}
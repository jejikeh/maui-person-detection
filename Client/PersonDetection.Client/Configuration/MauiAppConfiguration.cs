using System.Reflection;
using Camera.MAUI;
using CommunityToolkit.Maui;
using MauiIcons.FontAwesome;
using MauiIcons.Material;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using PersonDetection.Client.Pages;

namespace PersonDetection.Client.Configuration;

public static class MauiAppConfiguration
{
    public static MauiAppBuilder Configure(this MauiAppBuilder builder)
    {
        builder
            .UseMauiApp<App>()
            .AddConfiguration()
            .UseMauiCommunityToolkit()
            .UseFontAwesomeMauiIcons()
            .UseMaterialMauiIcons()
            .UseMauiCameraView()
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
            });

        #if DEBUG
            builder.Logging.AddDebug();
        #endif
        
        return builder;
    }

    public static void RegisterRoutes()
    {
        Routing.RegisterRoute(nameof(ChoosePhotoPage), typeof(ChoosePhotoPage));
        Routing.RegisterRoute(nameof(PhotoPage), typeof(PhotoPage));
        Routing.RegisterRoute(nameof(StreamCameraPage), typeof(StreamCameraPage));
    }

    private static MauiAppBuilder AddConfiguration(this MauiAppBuilder builder)
    {
        var platform = DeviceInfo.Current.Platform.ToString();
        var config = new ConfigurationBuilder()
            .AddJsonStream(GetManifestResourceStream("PersonDetection.Client.appsettings.json"))
            .AddJsonStream(GetManifestResourceStream($"PersonDetection.Client.appsettings.{platform}.json"))
            .Build();
        
        builder.Configuration.AddConfiguration(config);

        return builder;
    }

    private static Stream GetManifestResourceStream(string name)
    {
        var executingAssembly = Assembly.GetExecutingAssembly();
        var stream = executingAssembly.GetManifestResourceStream(name);
        
        if (stream is null)
        {
            throw new Exception($"{name} not found");
        }
        
        return stream;
    }
}
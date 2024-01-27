using Camera.MAUI;
using CommunityToolkit.Maui;
using MauiIcons.FontAwesome;
using MauiIcons.Material;
using Microsoft.Extensions.Logging;
using PersonDetection.Client.Pages;

namespace PersonDetection.Client.Configuration;

public static class MauiAppConfiguration
{
    public static MauiAppBuilder Configure(this MauiAppBuilder builder)
    {
        var m = builder.UseMauiApp<App>()
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
}
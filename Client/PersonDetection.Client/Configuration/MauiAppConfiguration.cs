using CommunityToolkit.Maui;
using MauiIcons.FontAwesome;
using Microsoft.Extensions.Logging;

namespace PersonDetection.Client.Configuration;

public static class MauiAppConfiguration
{
    public static MauiAppBuilder Configure(this MauiAppBuilder builder)
    {
        builder
            .UseMauiApp<App>()
            .UseMauiCommunityToolkit()
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
            });
        
        builder.UseMauiApp<App>().UseFontAwesomeMauiIcons();
        
        #if DEBUG
                builder.Logging.AddDebug();
        #endif
        
        return builder;
    }
}
using Microsoft.Extensions.Logging;

namespace PersonDetection.Client.Configuration;

public static class MauiAppConfiguration
{
    public static MauiAppBuilder Configure(this MauiAppBuilder builder)
    {
        builder
            .UseMauiApp<App>()
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
}
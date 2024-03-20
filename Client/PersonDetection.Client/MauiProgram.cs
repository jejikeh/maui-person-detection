using PersonDetection.Client.Configuration;

namespace PersonDetection.Client;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder().Configure();
        builder.AddInjections(builder.Configuration);
        
        return builder.Build();
    }
}
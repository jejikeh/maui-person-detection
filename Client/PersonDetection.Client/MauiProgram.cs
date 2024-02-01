using Microsoft.Maui.Hosting;
using PersonDetection.Client.Configuration;

namespace PersonDetection.Client;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();
        builder.Configure();
        builder.Services.AddInjections(builder.Configuration);
        
        return builder.Build();
    }
}
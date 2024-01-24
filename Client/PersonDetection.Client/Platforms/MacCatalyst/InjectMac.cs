using PersonDetection.Client.Application.Services;
using PersonDetection.Client.Platforms.Android.Services;

namespace PersonDetection.Client;

public static class InjectMac
{
    public static IServiceCollection UseMacServices(this IServiceCollection services)
    {
        #if MACCATALYST
                services.AddSingleton<IPlatformFilePicker, AndroidFilePicker>();
        #endif
        
        return services;
    }
}
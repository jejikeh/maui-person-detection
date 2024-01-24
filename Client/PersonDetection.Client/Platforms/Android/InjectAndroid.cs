using PersonDetection.Client.Application.Services;
using PersonDetection.Client.Platforms.Android.Services;

namespace PersonDetection.Client;

public static class InjectAndroid
{
    public static IServiceCollection UseAndroidServices(this IServiceCollection services)
    {
        #if ANDROID
        services.AddSingleton<IPlatformFilePicker, AndroidFilePicker>();
        #endif
        
        return services;
    }
}
using PersonDetection.Client.Application.Services;

#if ANDROID
using PersonDetection.Client.Platforms.Android.Services;
#endif

#if MACCATALYST
    using PersonDetection.Client.Platforms.MacCatalyst.Services;
#endif

namespace PersonDetection.Client.Configuration;

public static class PlatformServiceImplementationsInjection
{
    public static IServiceCollection AddPlatformServiceImplementations(this IServiceCollection serviceCollection)
    {
        return serviceCollection
            .AddAndroidServiceImplementations()
            .AddMacServiceImplementations();
    }

    private static IServiceCollection AddAndroidServiceImplementations(this IServiceCollection serviceCollection)
    {
        #if ANDROID
            serviceCollection.AddSingleton<IPlatformFilePicker, AndroidFilePicker>();
        #endif
        
        return serviceCollection;
    }

    private static IServiceCollection AddMacServiceImplementations(this IServiceCollection serviceCollection)
    {
        #if MACCATALYST
                serviceCollection.AddSingleton<IPlatformFilePicker, MacFilePicker>();
        #endif
        
        
        return serviceCollection;
    }
}
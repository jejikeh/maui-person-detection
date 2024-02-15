using Microsoft.Extensions.Configuration;
using PersonDetection.Client.Application.Services;
using PersonDetection.Client.Common;
using PersonDetection.Client.Infrastructure;
using PersonDetection.Client.Services;

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
    
    public static IServiceCollection AddPhotoProcessServices(
        this IServiceCollection serviceCollection,
        IConfiguration configuration,
        PhotoProcessProvider photoProcessProvider)
    {
        return photoProcessProvider switch
        {
            #if MACCATALYST
                // MAC CATALYST Doesn't support YoloV5
                PhotoProcessProvider.YoloV5 => serviceCollection.AddHttpClientProviders(),
            #else
                PhotoProcessProvider.YoloV5 => serviceCollection.AddYoloServices(configuration),
            #endif
                PhotoProcessProvider.Http => serviceCollection.AddHttpClientProviders(),
                _ => throw new ArgumentOutOfRangeException()
        };
    }

    private static IServiceCollection AddAndroidServiceImplementations(this IServiceCollection serviceCollection)
    {
        #if ANDROID
            serviceCollection.AddSingleton<IPlatformFilePicker, AndroidFilePicker>();
            serviceCollection.AddSingleton<IPlatformImageSourceLoader, AndroidImageSourceLoader>();
        #endif
        
        return serviceCollection;
    }

    private static IServiceCollection AddMacServiceImplementations(this IServiceCollection serviceCollection)
    {
        #if MACCATALYST
            serviceCollection.AddSingleton<IPlatformFilePicker, MacFilePicker>();
            serviceCollection.AddSingleton<IPlatformImageSourceLoader, MacImageSourceLoader>();
        #endif
        
        return serviceCollection;
    }
}
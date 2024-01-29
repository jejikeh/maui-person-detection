using Microsoft.Extensions.DependencyInjection.Extensions;
using PersonDetection.Client.Application.Services;
using PersonDetection.Client.Infrastructure;
using PersonDetection.Client.Infrastructure.Common;
using PersonDetection.Client.Infrastructure.Services;
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
        PhotoProcessProvider photoProcessProvider)
    {
        return photoProcessProvider switch
        {
            #if MACCATALYST
                PhotoProcessProvider.YoloV5 => serviceCollection.AddHttpClientProviders(),
                PhotoProcessProvider.Http => serviceCollection.AddHttpClientProviders(),
                _ => throw new ArgumentOutOfRangeException()
            #else
                PhotoProcessProvider.YoloV5 => serviceCollection.AddYoloServices(),
                PhotoProcessProvider.Http => serviceCollection.AddHttpClientProviders(),
                _ => throw new ArgumentOutOfRangeException()
            #endif
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
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using PersonDetection.Client.Application.Services;
using PersonDetection.Client.Common;
using PersonDetection.Client.Infrastructure;
using PersonDetection.Client.Infrastructure.Services;
using PersonDetection.Client.Services;

#if ANDROID
    using PersonDetection.Client.Platforms.Android.Services;
    using Neural.Defaults;
    using Neural.Onnx.Common;
    using Neural.Onnx.Common.Options;
    using PersonDetection.Client.Common.Options;
#endif

#if MACCATALYST
    using PersonDetection.Client.Platforms.MacCatalyst.Services;
#endif

namespace PersonDetection.Client.Configuration;

public static class PlatformServiceImplementationsInjection
{
    public static IServiceCollection AddPlatformServiceImplementations(this MauiAppBuilder builder)
    {
        return builder
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
                PhotoProcessProvider.YoloV5 => serviceCollection.AddSingleton<IPhotoProcessService, OnnxPhotoProcessService>(),
            #endif
                PhotoProcessProvider.Http => serviceCollection.AddHttpClientProviders(),
                _ => throw new ArgumentOutOfRangeException(nameof(photoProcessProvider))
        };
    }
    
    #if ANDROID
    
    private static MauiAppBuilder ConfigureNeuralHub(this MauiAppBuilder builder)
    {
        var onnxOptions = builder.Services.GetConfigureOptions<OnnxOptions>(builder.Configuration);
        var painterOptions = builder.Services.GetConfigureOptions<ImageBoxPainterOptions>(builder.Configuration);
        
        var modelCount = Environment.ProcessorCount / 2;

        var neuralHubBuilder = NeuralHubConfiguration
            .FromDefaults(
                fileSystemProvider: new MauiFileSystemStreamProvider());

        neuralHubBuilder
            .AddYolo5Models(LoadModelStream(onnxOptions), modelCount)
            .AddImageBoxPainterModels(painterOptions, Environment.ProcessorCount);

        builder.Services.AddSingleton(neuralHubBuilder.Build());

        builder.Services.AddSingleton<OnnxNeuralService>();

        return builder;
    }
    
    private static byte[] LoadModelStream(OnnxOptions onnxOptions) 
    {
        var mauiFileSystemStreamProvider = new MauiFileSystemStreamProvider();
        
        var stream = mauiFileSystemStreamProvider.GetFileStreamAsync(onnxOptions.Yolo5OnnxModelPath).Result;
        
        using var modelStream = new MemoryStream();
        
        stream.CopyTo(modelStream);

        return modelStream.ToArray();
    }
    
    #endif

    private static IServiceCollection AddAndroidServiceImplementations(this MauiAppBuilder builder)
    {
        #if ANDROID
            builder.Services.AddSingleton<IPlatformFilePicker, AndroidFilePicker>();
            builder.Services.AddSingleton<IPlatformImageSourceLoader, AndroidImageSourceLoader>();
            
            builder.ConfigureNeuralHub();
        #endif
        
        return builder.Services;
    }

    private static IServiceCollection AddMacServiceImplementations(this IServiceCollection serviceCollection)
    {
        #if MACCATALYST
            serviceCollection.AddSingleton<IPlatformFilePicker, MacFilePicker>();
            serviceCollection.AddSingleton<IPlatformImageSourceLoader, MacImageSourceLoader>();
        #endif
        
        return serviceCollection;
    }
    
    private static T GetConfigureOptions<T>(this IServiceCollection serviceCollection, IConfiguration configuration) where T : class, new()
    {
        var options = new T();
        configuration.GetSection(typeof(T).Name).Bind(options);
        serviceCollection.AddSingleton(Options.Create(options));

        return options;
    }
}
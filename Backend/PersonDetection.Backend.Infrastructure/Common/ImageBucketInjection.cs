using Microsoft.Extensions.DependencyInjection;
using Minio.AspNetCore;
using PersonDetection.Backend.Infrastructure.Common.Options;
using PersonDetection.Backend.Infrastructure.Services;
using PersonDetection.Backend.Infrastructure.Services.Implementations;

namespace PersonDetection.Backend.Infrastructure.Common;

public static class ImageBucketInjection
{
    public static IServiceCollection AddImageBucket(this IServiceCollection serviceCollection, ImageBucketOptions imageBucketOptions)
    {
        serviceCollection.AddMinio(options =>
        {
            options.Endpoint = imageBucketOptions.Endpoint;
            options.AccessKey = imageBucketOptions.AccessKey;
            options.SecretKey = imageBucketOptions.SecretKey;
        });
            
        serviceCollection.AddSingleton<IImageBucketService, ImageBucketService>();

        return serviceCollection;
    }
}
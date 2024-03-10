using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using PersonDetection.Backend.Infrastructure.Common;
using PersonDetection.Backend.Infrastructure.Common.Options;

namespace PersonDetection.Backend.Infrastructure;

public static class InfrastructureInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection serviceCollection, IConfiguration configuration)
    {
        InjectIdentityDb(serviceCollection, configuration);
        InjectImageBucket(serviceCollection, configuration);

        return serviceCollection;
    }

    private static void InjectImageBucket(IServiceCollection serviceCollection, IConfiguration configuration)
    {
        var imageBucketOptions = serviceCollection.GetConfigureOptions<ImageBucketOptions>(configuration);
        
        serviceCollection.AddImageBucket(imageBucketOptions);
    }

    private static void InjectIdentityDb(IServiceCollection serviceCollection, IConfiguration configuration)
    {
        var personDetectionContextOptions = serviceCollection.GetConfigureOptions<PersonDetectionContextOptions>(configuration);
        var identityOptions = serviceCollection.GetConfigureOptions<IdentityModelOptions>(configuration);

        serviceCollection
            .AddInfrastructureOptions(configuration)
            .UseIdentityServices(identityOptions)
            .AddDbContext(personDetectionContextOptions);
    }

    private static T GetConfigureOptions<T>(this IServiceCollection serviceCollection, IConfiguration configuration) where T : class, new()
    {
        var options = new T();
        configuration.GetSection(typeof(T).Name).Bind(options);
        serviceCollection.AddSingleton(Options.Create(options));

        return options;
    }
}
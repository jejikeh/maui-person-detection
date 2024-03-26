using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using PersonDetection.Backend.Infrastructure.Common.Options;
using PersonDetection.Backend.Infrastructure.Services;
using PersonDetection.ImageProcessing.Services;

namespace PersonDetection.Backend.Infrastructure;

public static class InfrastructureInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection serviceCollection, IConfiguration configuration)
    {
        var personDetectionContextOptions = serviceCollection.GetConfigureOptions<PersonDetectionContextOptions>(configuration);
        var identityOptions = serviceCollection.GetConfigureOptions<IdentityModelOptions>(configuration);

        return serviceCollection
            .AddSingleton<IFileSystemStreamProvider, CoreFileSystemStreamProvider>()
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

    private static IServiceCollection AddDbContext(this IServiceCollection serviceCollection, PersonDetectionContextOptions options)
    {
        return serviceCollection.AddDbContext<PersonDetectionDbContext>(builder =>
        {
            builder.UseNpgsql(options.ConnectionString);
        });
    }
    
    private static IServiceCollection UseIdentityServices(this IServiceCollection services, IdentityModelOptions identityOptions)
    {
        services.AddIdentity<IdentityUser, IdentityRole>(options =>
            {
                options.User = identityOptions.User;
                options.Password = identityOptions.Password;
                options.SignIn = identityOptions.SignIn;
            })
            .AddEntityFrameworkStores<PersonDetectionDbContext>()
            .AddDefaultTokenProviders()
            .AddRoles<IdentityRole>();

        return services;
    }
    
    private static IServiceCollection AddInfrastructureOptions(this IServiceCollection serviceCollection, IConfiguration configuration)
    {
        return serviceCollection
            .ConfigureOptions<PersonDetectionContextOptions>(configuration);
    }

    private static IServiceCollection ConfigureOptions<T>(this IServiceCollection serviceCollection, IConfiguration configuration) where T : class
    {
        return serviceCollection.Configure<T>(configuration.GetSection(typeof(T).Name));
    }
}
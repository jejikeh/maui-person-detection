using System.Reflection;
using FluentValidation;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using PersonDetection.Backend.Application.Services;
using PersonDetection.ImageProcessing;

namespace PersonDetection.Backend.Application;

public static class ApplicationInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection serviceCollection, IConfiguration configuration)
    {
        return serviceCollection
            .AddYoloImageProcessing(configuration)
            .AddScoped<IAuthorizationService, AuthorizationService>()
            .AddSingleton<PhotoProcessingService>()
            .AddValidations();
    }

    private static IServiceCollection AddValidations(this IServiceCollection serviceCollection)
    {
        return serviceCollection.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
    }
    
    private static IServiceCollection ConfigureOptions<T>(this IServiceCollection serviceCollection, IConfiguration configuration) where T : class
    {
        return serviceCollection.Configure<T>(configuration.GetSection(typeof(T).Name));
    }
    
    private static T GetConfigureOptions<T>(this IServiceCollection serviceCollection, IConfiguration configuration) where T : class, new()
    {
        var options = new T();
        configuration.GetSection(typeof(T).Name).Bind(options);
        serviceCollection.AddSingleton(Options.Create(options));

        return options;
    }
}
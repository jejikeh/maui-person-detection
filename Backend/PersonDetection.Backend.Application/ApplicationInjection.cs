using System.Reflection;
using FluentValidation;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PersonDetection.Backend.Application.Services;
using PersonDetection.Backend.Application.Services.Implementations;
using PersonDetection.ImageSegmentation.ModelConverter;

namespace PersonDetection.Backend.Application;

public static class ApplicationInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection serviceCollection, IConfiguration configuration)
    {
        return serviceCollection
            .AddYoloImageSegmentationProcessing(configuration)
            .AddSingleton<ModelTypeProvider>()
            .AddScoped<IAuthorizationService, AuthorizationService>()
            .AddSingleton<IPhotoProcessingService, PhotoProcessingService>()
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
}
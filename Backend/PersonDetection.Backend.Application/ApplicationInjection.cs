using System.Reflection;
using FluentValidation;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PersonDetection.Backend.Application.Services;
using PersonDetection.ImageSegmentation.ModelConverter;

namespace PersonDetection.Backend.Application;

public static class ApplicationInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection serviceCollection, IConfiguration configuration)
    {
        return serviceCollection
            .AddYoloImageSegmentationProcessing()
            .AddScoped<IAuthorizationService, AuthorizationService>()
            .AddSingleton<PhotoProcessingService>()
            .AddValidations();
    }

    private static IServiceCollection AddValidations(this IServiceCollection serviceCollection)
    {
        return serviceCollection.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
    }
}
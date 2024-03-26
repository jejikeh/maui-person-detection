using System.Reflection;
using FluentValidation;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Neural.Onnx.Services;
using Neural.Onnx.Services.Implementations;
using PersonDetection.Backend.Application.Services;
using PersonDetection.Backend.Application.Services.Implementations;
using PersonDetection.ImageSegmentation.ModelConverter;

namespace PersonDetection.Backend.Application;

public static class ApplicationInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection serviceCollection, IConfiguration configuration)
    {
        return serviceCollection
            .AddSingleton<ModelTypeProvider>()
            .AddSingleton<IOnnxNeuralService, OnnxNeuralService>()
            .AddScoped<IAuthorizationService, AuthorizationService>()
            .AddSingleton<IPhotoProcessingService, PhotoProcessingService>()
            .AddValidations();
    }

    private static IServiceCollection AddValidations(this IServiceCollection serviceCollection)
    {
        return serviceCollection.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
    }
}
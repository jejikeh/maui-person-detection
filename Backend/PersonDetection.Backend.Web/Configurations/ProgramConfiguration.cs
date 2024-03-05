using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Neural.Defaults;
using Neural.Onnx.Common;
using Neural.Onnx.Common.Options;
using PersonDetection.Backend.Application;
using PersonDetection.Backend.Infrastructure;
using PersonDetection.Backend.Web.Common;
using PersonDetection.Backend.Web.Endpoints;
using PersonDetection.Backend.Web.Hubs;
using PersonDetection.Backend.Web.Middlewares;
using PersonDetection.Backend.Web.Services;
using PersonDetection.Backend.Web.Services.Implementations;

namespace PersonDetection.Backend.Web.Configurations;

public static class ProgramConfiguration
{
    private const long _signalRMaximumReceiveMessageSize = 1024 * 1024 * 10;
    private const string _allowFrontendPolicyName = "_allowFrontend";

    public static WebApplicationBuilder Configure(this WebApplicationBuilder builder)
    {
        builder.ConfigureJsonOptions();
        
        builder.ConfigureCors();
        
        builder.ConfigureErrorHandling();

        builder.ConfigureServices();
        
        builder.Services.AddAuthorization();
            
        return builder;
    }

    private static WebApplicationBuilder ConfigureJsonOptions(this WebApplicationBuilder builder)
    {
        builder.Services.Configure<JsonOptions>(options =>
        {
            options.JsonSerializerOptions.WriteIndented = true;
        });

        return builder;
    }

    private static WebApplicationBuilder ConfigureServices(this WebApplicationBuilder builder)
    {
        builder
            .ConfigureSignalR()
            .ConfigureNeuralHub()
            .ConfigureApplicationLayers();
        
        builder.Services.AddOptions();
        
        builder.Services.AddSingleton<IVideoPredictionsChannelService, VideoPredictionsChannelService>();

        return builder;
    }

    private static WebApplicationBuilder ConfigureSignalR(this WebApplicationBuilder builder)
    {
        builder.Services.AddSignalR(options =>
        { 
            options.EnableDetailedErrors = true;
            options.MaximumReceiveMessageSize = _signalRMaximumReceiveMessageSize;
        });

        return builder;
    }

    private static WebApplicationBuilder ConfigureApplicationLayers(this WebApplicationBuilder builder)
    {
        builder.Services.AddApplication(builder.Configuration)
            .AddInfrastructure(builder.Configuration);

        return builder;
    }

    private static void ConfigureErrorHandling(this WebApplicationBuilder builder)
    {
        builder.Services
            .AddTransient<GlobalExceptionHandlerMiddleware>()
            .AddProblemDetails();
    }

    private static WebApplicationBuilder ConfigureNeuralHub(this WebApplicationBuilder builder)
    {
        var onnxOptions = builder.Services.GetConfigureOptions<OnnxOptions>(builder.Configuration);
        var painterOptions = builder.Services.GetConfigureOptions<ImageBoxPainterOptions>(builder.Configuration);
        
        var modelCount = Environment.ProcessorCount / 2;
        
        var neuralHubBuilder = NeuralHubConfiguration.FromDefaults();
            
        neuralHubBuilder
            .AddYolo8Models(onnxOptions.Yolo8OnnxModelPath, modelCount)
            .AddImageSegmentationPainterModels(painterOptions, Environment.ProcessorCount);
            
        neuralHubBuilder
            .AddYolo5Models(onnxOptions.Yolo5OnnxModelPath, modelCount)
            .AddImageBoxPainterModels(painterOptions, Environment.ProcessorCount)
            .Build();

        builder.Services.AddSingleton(neuralHubBuilder.Build());

        return builder;
    }

    private static WebApplicationBuilder ConfigureCors(this WebApplicationBuilder builder)
    {
        var frontendOptions = builder.Services.GetConfigureOptions<FrontendOptions>(builder.Configuration);

        builder.Services.AddCors(options =>
        {
            options.AddPolicy(_allowFrontendPolicyName, policy =>
            {
                policy
                    .WithOrigins(frontendOptions.Host)
                    .AllowCredentials()
                    .AllowAnyMethod()
                    .AllowAnyHeader();
            });
        });
        
        return builder;
    }

    public static WebApplication Configure(this WebApplication app)
    {
        app.UseMiddleware<GlobalExceptionHandlerMiddleware>();
        app.UseCors(_allowFrontendPolicyName);
        app.UseAuthentication();
        app.UseAuthorization();
        app.MapHub<VideoHub>("/video");
        app.MapEndpoints();
        
        return app;
    }
    
    public static async Task<WebApplication> RunAppAsync(this WebApplication app)
    {
        using var scope = app.Services.CreateScope();
        var serviceProvider = scope.ServiceProvider;
        
        try
        {
            var identityDbContext = serviceProvider.GetRequiredService<PersonDetectionDbContext>();
            await identityDbContext.Database.MigrateAsync();
            
            await app.RunAsync();
        }
        catch (Exception ex)
        {
            Console.WriteLine("Host terminated unexpectedly");
            Console.WriteLine(ex);
        }
        
        return app;
    }

    private static WebApplication MapEndpoints(this WebApplication app)
    {
        app.MapGet("user", IdentifyEndpoint.Handler);
        app.MapPost("login", LoginEndpoint.HandlerAsync);
        app.MapPost("logout", LogoutEndpoint.Handler).RequireAuthorization();
        app.MapPost("register", RegisterEndpoint.HandlerAsync);
        app.MapPost("photo", PhotoEndpoint.HandlerAsync);

        return app;
    }
    
    private static T GetConfigureOptions<T>(this IServiceCollection serviceCollection, IConfiguration configuration) where T : class, new()
    {
        var options = new T();
        configuration.GetSection(typeof(T).Name).Bind(options);
        serviceCollection.AddSingleton(Options.Create(options));

        return options;
    }
} 
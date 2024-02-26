using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Neural.Core;
using Neural.Defaults;
using Neural.Onnx.Common;
using PersonDetection.Backend.Application;
using PersonDetection.Backend.Application.Services;
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
    private static readonly string _allowFrontendPolicyName = "_allowFrontend";
    
    public static WebApplicationBuilder Configure(this WebApplicationBuilder builder)
    {
        builder.Services.Configure<JsonOptions>(options => { options.JsonSerializerOptions.WriteIndented = true; });
        
        builder.ConfigureCors();

        builder.ConfigureNeuralHub();

        builder.Services.AddOptions();
        
        builder.ConfigureErrorHandling();
        
        builder.Services.AddSingleton<IVideoPredictionsChannelService, VideoPredictionsChannelService>();
        
        builder.ConfigureApplicationLayers();
        
        builder.Services.AddAuthorization();
            
        builder.ConfigureSignalR();

        return builder;
    }

    private static void ConfigureSignalR(this WebApplicationBuilder builder)
    {
        builder.Services.AddSignalR(options =>
        { 
            options.EnableDetailedErrors = true;
            options.MaximumReceiveMessageSize = 1024 * 1024 * 10;
        });
    }

    private static void ConfigureApplicationLayers(this WebApplicationBuilder builder)
    {
        builder.Services.AddApplication(builder.Configuration)
            .AddInfrastructure(builder.Configuration);
    }

    private static void ConfigureErrorHandling(this WebApplicationBuilder builder)
    {
        builder.Services
            .AddTransient<GlobalExceptionHandlerMiddleware>()
            .AddProblemDetails();
    }

    private static void ConfigureNeuralHub(this WebApplicationBuilder builder)
    {
        var neuralHub = NeuralHubConfiguration
            .FromDefaults()
            .AddYolo5Models("Weights/yolov5n.onnx", 25)
            .AddImageBoxPainterModels(25)
            .Build();

        builder.Services.AddSingleton(neuralHub);
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
        app.MapGet("identify", IdentifyEndpoint.Handler);
        app.MapPost("login", LoginEndpoint.HandlerAsync);
        app.MapPost("logout", LogoutEndpoint.Handler).RequireAuthorization();
        app.MapPost("register", RegisterEndpoint.HandlerAsync);
        app.MapPost("photo", PhotoEndpoint.HandlerAsync);
        app.MapPost("model/switch", SwitchModelTypeEndpoint.HandlerAsync);

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
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using PersonDetection.Backend.Application;
using PersonDetection.Backend.Application.Services;
using PersonDetection.Backend.Infrastructure;
using PersonDetection.Backend.Web.Common;
using PersonDetection.Backend.Web.Endpoints;
using PersonDetection.Backend.Web.Hubs;
using PersonDetection.Backend.Web.Middlewares;

namespace PersonDetection.Backend.Web.Configurations;

public static class ProgramConfiguration
{
    private static readonly string _allowFrontendPolicyName = "_allowFrontend";
    
    public static WebApplicationBuilder Configure(this WebApplicationBuilder builder)
    {
        builder.Services.Configure<JsonOptions>(options => { options.JsonSerializerOptions.WriteIndented = true; });

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

        builder.Services
            .AddTransient<GlobalExceptionHandlerMiddleware>()
            .AddSingleton<VideoPredictionsChannelService>()
            .AddProblemDetails()
            .AddApplication(builder.Configuration)
            .AddInfrastructure(builder.Configuration)
            .AddAuthorization()
            .AddSignalR(options =>
            {
                options.EnableDetailedErrors = true;
                options.MaximumReceiveMessageSize = 1024 * 1024 * 10;
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
        app.MapPost("login", LoginEndpoint.Handler);
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
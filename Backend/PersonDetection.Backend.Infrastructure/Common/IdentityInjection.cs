using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PersonDetection.Backend.Infrastructure.Common.Options;
using PersonDetection.Backend.Infrastructure.Models;

namespace PersonDetection.Backend.Infrastructure.Common;

public static class IdentityInjection
{
    public static IServiceCollection AddDbContext(this IServiceCollection serviceCollection, PersonDetectionContextOptions options)
    {
        return serviceCollection.AddDbContext<PersonDetectionDbContext>(builder =>
        {
            builder.UseNpgsql(options.ConnectionString);
        });
    }
    
    public static IServiceCollection UseIdentityServices(this IServiceCollection services, IdentityModelOptions identityOptions)
    {
        services.AddIdentity<User, IdentityRole>(options =>
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
    
    public static IServiceCollection AddInfrastructureOptions(this IServiceCollection serviceCollection, IConfiguration configuration)
    {
        return serviceCollection
            .ConfigureOptions<PersonDetectionContextOptions>(configuration);
    }

    private static IServiceCollection ConfigureOptions<T>(this IServiceCollection serviceCollection, IConfiguration configuration) where T : class
    {
        return serviceCollection.Configure<T>(configuration.GetSection(typeof(T).Name));
    }
}
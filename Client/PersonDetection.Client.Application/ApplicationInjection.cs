using PersonDetection.Client.Application.ViewModels;

namespace PersonDetection.Client.Application;

public static class ApplicationInjection
{
    public static IServiceCollection UseApplication(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddTransient<ChoosePhotoViewModel>();
        
        return serviceCollection;
    }
}


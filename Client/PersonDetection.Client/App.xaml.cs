using PersonDetection.Client.Configuration;
using PersonDetection.Client.Services;

namespace PersonDetection.Client;

public partial class App
{
    public App(ClientConfiguration clientConfiguration, IExceptionHandler exceptionHandler)
    {
        InitializeComponent();
        MainPage = new AppShell();

        if (clientConfiguration.UseExceptionHandler)
        {
            // @Note: This is not working on Android, and on MacCatalyst there are strange issues (Exceptions can be thrown more than once).
            // - https://github.com/dotnet/maui/discussions/653?sort=top
            // I can use Sentry here, but it only for logging and not for displaying UI.
            AppDomain.CurrentDomain.FirstChanceException += exceptionHandler.OnException!;
        }
    }
}
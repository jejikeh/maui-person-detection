using Microsoft.Extensions.Options;
using PersonDetection.Client.Common.Options;
using PersonDetection.Client.Services;

namespace PersonDetection.Client;

public partial class App
{
    public App(IOptions<ClientOptions> options, IExceptionHandler exceptionHandler)
    {
        InitializeComponent();
        MainPage = new AppShell();

        if (options.Value.UseExceptionHandler)
        {
            // This code is not functioning correctly on Android, and on MacCatalyst, there are peculiar issues (exceptions can be thrown more than once).
            // - https://github.com/dotnet/maui/discussions/653?sort=top
            // While I can use Sentry for logging purposes, it is not designed for displaying UI.
            AppDomain.CurrentDomain.FirstChanceException += exceptionHandler.OnException!;
        }
    }
}
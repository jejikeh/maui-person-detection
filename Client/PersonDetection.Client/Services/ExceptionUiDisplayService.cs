using System.Runtime.ExceptionServices;
using CommunityToolkit.Maui.Alerts;
using Microsoft.Extensions.Options;
using PersonDetection.Client.Common.Options;
using PersonDetection.Client.Configuration;
using PersonDetection.Client.Infrastructure.Common;
using MauiApplication = Microsoft.Maui.Controls.Application;

namespace PersonDetection.Client.Services;

public class ExceptionUiDisplayService(IOptions<ClientOptions> _clientOptions) : IExceptionHandler
{
    private readonly bool _displayAlerts = _clientOptions.Value.DisplayExceptionDetails;
    
    public void OnException(object sender, FirstChanceExceptionEventArgs firstChanceExceptionEventArgs)
    {
        if (_displayAlerts)
        {
            MauiApplication.Current?.Dispatcher.DispatchAsync(() => ShowAlert(firstChanceExceptionEventArgs.Exception));
        }
    }

    private static void ShowAlert(Exception ex) => 
        Toast.Make($"{ex.Message}").Show();
}
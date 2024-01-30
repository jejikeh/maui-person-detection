using System.Runtime.ExceptionServices;
using CommunityToolkit.Maui.Alerts;
using PersonDetection.Client.Configuration;
using PersonDetection.Client.Infrastructure.Common;
using MauiApplication = Microsoft.Maui.Controls.Application;

namespace PersonDetection.Client.Services;

public class ExceptionUiDisplayService(ClientConfiguration clientConfiguration) : IExceptionHandler
{
    private readonly bool _displayAlerts = clientConfiguration.DisplayExceptionDetails;
    
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
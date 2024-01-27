using CommunityToolkit.Maui.Alerts;
using PersonDetection.Client.Application.Models.Types;

namespace PersonDetection.Client.Extensions;

public static class ErrorExtensions
{
    public static Task DisplayErrorAsync(this Error error)
    {
        return Shell.Current.DisplayAlert("Something went wrong", error.Message, "Ok");
    }

    public static Task ToastErrorAsync(this Error error)
    {
        return Toast.Make(error.Message).Show();
    }
}
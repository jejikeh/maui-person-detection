using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Camera.MAUI;
using CommunityToolkit.Maui.Alerts;
using PersonDetection.Client.ViewModels;

namespace PersonDetection.Client.Pages;

public partial class StreamCameraPage : ContentPage
{
    private readonly StreamCameraViewModel _streamCameraViewModel;
    
    public StreamCameraPage(StreamCameraViewModel streamCameraViewModel)
    {
        InitializeComponent();
        BindingContext = streamCameraViewModel;
        _streamCameraViewModel = streamCameraViewModel;
    }

    private async void CameraView_OnCamerasLoaded(object? sender, EventArgs e)
    {
        var status = await StreamCameraViewModel.CheckAndRequestCameraPermission();
        
        if (status != PermissionStatus.Granted)
        {
            return;
        }
        
        if (CameraView.Cameras.Count == 0)
        {
            await Toast.Make("No cameras found").Show();
        }
        
        CameraView.Camera = CameraView.Cameras.FirstOrDefault();
        MainThread.BeginInvokeOnMainThread(() => _streamCameraViewModel.StartCamera(CameraView));
    }

    protected override void OnDisappearing()
    {
        base.OnDisappearing();
        
        _streamCameraViewModel.CanSnapShot = true;
    }
}
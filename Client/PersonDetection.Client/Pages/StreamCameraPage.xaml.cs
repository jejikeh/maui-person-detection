using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Camera.MAUI;
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

    private void CameraView_OnCamerasLoaded(object? sender, EventArgs e)
    {
        if (CameraView.Cameras.Count == 0)
        {
            throw new Exception("No camera found");
        } 
        
        CameraView.Camera = CameraView.Cameras.FirstOrDefault();
        MainThread.BeginInvokeOnMainThread(() => _streamCameraViewModel.StartCamera(CameraView));
    }
}
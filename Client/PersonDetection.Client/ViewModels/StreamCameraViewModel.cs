using Camera.MAUI;
using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using PersonDetection.Client.Application.Models;
using PersonDetection.Client.Application.Services;
using PersonDetection.Client.Extensions;
using PersonDetection.Client.Models;
using PersonDetection.Client.Services;


namespace PersonDetection.Client.ViewModels;

public partial class StreamCameraViewModel(PhotoService photoService, IPlatformImageSourceLoader imageSourceLoader) : ObservableObject
{
    [ObservableProperty]
    private ViewPhotoPair _viewPhotoPair = default!;
    
    [ObservableProperty]
    private CameraView _cameraView = default!;
    
    [ObservableProperty]
    private CameraInfo _camera = default!;
    
    public async void StartCamera(CameraView cameraView)
    {
        await cameraView.StopCameraAsync();
        
        var result = await cameraView.StartCameraAsync();
        
        if (result != CameraResult.Success)
        {
            throw new Exception("Camera could not be started");
        }
        
        CameraView = cameraView;
        // @Note: These two lines are required for the camera to start on android
        cameraView.AutoSnapShotAsImageSource = true;
        cameraView.TakeAutoSnapShot = true;
    }

    [RelayCommand]
    private async Task TakeSnapshot()
    {
        var photo = await GetPhotoFromCamera();
        
        if (photo is null)
        {
            await Toast.Make("No image was taken").Show();
            return;
        }

        await Task.Run(async () =>
        {
            var processPhotoToGallery = await photoService.ProcessPhotoToGalleryAsync(photo);
            
            if (processPhotoToGallery.IsError)
            {
                await processPhotoToGallery.GetError().DisplayErrorAsync();
                return;
            }

            var photoTuple = processPhotoToGallery.GetValue();
            ViewPhotoPair = imageSourceLoader.LoadViewPhotoPair(photoTuple.Original, photoTuple.Processed);
        });
    }

    private async Task<Photo?> GetPhotoFromCamera()
    {
        var snapShot = (StreamImageSource)CameraView.GetSnapShot();
        
        if (snapShot is null)
        {
            return null;
        }

        var taskStream = snapShot.Stream(CancellationToken.None);
        var stream = taskStream.Result;

        var bytes = new byte[stream.Length];
        await stream.ReadAsync(bytes);

        var photo = new Photo
        {
            Content = Convert.ToBase64String(bytes)
        };
        
        return photo;
    }

    public static async Task<PermissionStatus> CheckAndRequestCameraPermission()
    {
        var status = await Permissions.CheckStatusAsync<Permissions.Camera>();

        switch (status)
        {
            case PermissionStatus.Granted:
                return status;
            case PermissionStatus.Denied when DeviceInfo.Platform == DevicePlatform.iOS:
                throw new Exception("Camera permission denied");
        }
        
        status = await Permissions.RequestAsync<Permissions.Camera>();

        return status;
    }
}
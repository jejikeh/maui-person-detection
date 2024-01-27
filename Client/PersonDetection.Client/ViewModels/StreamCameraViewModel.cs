using Camera.MAUI;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace PersonDetection.Client.ViewModels;

public partial class StreamCameraViewModel : ObservableObject
{
    [ObservableProperty]
    private ImageSource _snapshotImage;
    
    [ObservableProperty]
    private CameraInfo _camera;
    
    private CameraView _cameraView;

    public async void StartCamera(CameraView cameraView)
    {
        var status = await CheckAndRequestCameraPermission();
        if (status != PermissionStatus.Granted)
        {
            return;
        }
        
        var result = await cameraView.StartCameraAsync();
        // On some devices, the camera could not be started first time.
        if (result != CameraResult.Success)
        {
            await cameraView.StopCameraAsync();
            result = await cameraView.StartCameraAsync();
        }

        if (result != CameraResult.Success)
        {
            throw new Exception("Camera could not be started");
        }
        
        _cameraView = cameraView;
        cameraView.AutoSnapShotAsImageSource = true;
        cameraView.TakeAutoSnapShot = true;
        SnapshotImage = cameraView.GetSnapShot();
    }

    [RelayCommand]
    public async Task TakeSnapshot()
    {
        SnapshotImage = _cameraView.GetSnapShot();
    }
    
    private async Task<PermissionStatus> CheckAndRequestCameraPermission()
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
using PersonDetection.Client.Application.Models;

namespace PersonDetection.Client.Application.Services;

public interface IPlatformFilePicker
{ 
    public Task<Photo?> PickPhotoAsync();
}
using PersonDetection.Client.Application.Models;
using PersonDetection.Client.Application.Models.Types;

namespace PersonDetection.Client.Application.Services;

public interface IPlatformFilePicker
{ 
    public Task<Result<Photo, Error>> PickPhotoAsync();
}
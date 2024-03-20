// To fix IDE errors.
#if ANDROID
using PersonDetection.Client.Application.Models;
using PersonDetection.Client.Application.Models.Types;
using PersonDetection.Client.Application.Services;
using PersonDetection.Client.Infrastructure.Common;

namespace PersonDetection.Client.Platforms.Android.Services;

public class OnnxPhotoProcessService(OnnxNeuralService _onnxNeuralService) : IPhotoProcessService
{
    public async Task<Result<Photo, Error>> ProcessPhotoAsync(Photo originalPhoto, CancellationToken cancellationToken = default)
    {
        var predictedPhoto = await _onnxNeuralService.ProcessPhotoAsync(originalPhoto.Content);
        
        if (predictedPhoto is null)
        {
            return new Error(InfrastructureErrorMessages.FailedToProcessPhoto);
        }
        
        return new Photo()
        {
            Content = predictedPhoto,
        };
    }
}
#endif
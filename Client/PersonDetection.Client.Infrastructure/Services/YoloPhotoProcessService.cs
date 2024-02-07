using PersonDetection.Client.Application.Models;
using PersonDetection.Client.Application.Models.Types;
using PersonDetection.Client.Application.Services;
using PersonDetection.Client.Infrastructure.Common;
using PersonDetection.ImageProcessing;

namespace PersonDetection.Client.Infrastructure.Services;

public class YoloPhotoProcessService(YoloImageProcessing yoloImageProcessing) : IPhotoProcessService
{
    public async Task<Result<Photo, Error>> ProcessPhotoAsync(Photo originalPhoto, CancellationToken cancellationToken = default)
    {
        var predictedPhoto = await yoloImageProcessing.PredictAsync(originalPhoto.Content);
        
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
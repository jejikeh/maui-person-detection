using PersonDetection.Client.Application.Models;
using PersonDetection.Client.Application.Services;
using PersonDetection.Client.Infrastructure.Common;
using PersonDetection.Client.Infrastructure.Dto;

namespace PersonDetection.Client.Infrastructure.Services;

public class HttpPhotoProcessService(
    CacheHttpClientService httpClient, 
    IInfrastructureConfiguration configuration) : IPhotoProcessService
{
    public async Task<Photo?> ProcessPhotoAsync(Photo originalPhoto, CancellationToken cancellationToken = default)
    {
        // var result = await httpClient.GetAsync<PhotoProcessResultDto>(configuration.PhotoProcessUrl + "photo", cancellationToken);
        // if (result is null || string.IsNullOrEmpty(result.Content))
        // {
            // return null;
        // }

        originalPhoto.FileUrl = string.Empty;
        
        return originalPhoto;
    }
}
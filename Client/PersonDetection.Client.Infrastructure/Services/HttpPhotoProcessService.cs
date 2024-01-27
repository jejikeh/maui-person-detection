using PersonDetection.Client.Application.Models;
using PersonDetection.Client.Application.Models.Types;
using PersonDetection.Client.Application.Services;
using PersonDetection.Client.Infrastructure.Common;
using PersonDetection.Client.Infrastructure.Dto;

namespace PersonDetection.Client.Infrastructure.Services;

public class HttpPhotoProcessService(
    CacheHttpClientService httpClient, 
    IInfrastructureConfiguration configuration) : IPhotoProcessService
{
    public async Task<Result<Photo, Error>> ProcessPhotoAsync(Photo originalPhoto, CancellationToken cancellationToken = default)
    {
        var result = await httpClient.PostAsync(
            configuration.PhotoProcessUrl,
            new PhotoProcessResultDto
            {
                Content = originalPhoto.Content
            },
            cancellationToken);
        
        if (result.IsError)
        {
            return result.GetError();
        }

        return new Photo()
        {
            Content = result.GetValue().Content,
        };
    }
}
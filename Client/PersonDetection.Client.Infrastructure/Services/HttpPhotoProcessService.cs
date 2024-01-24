using PersonDetection.Client.Application.Dto;
using PersonDetection.Client.Application.Models;
using PersonDetection.Client.Application.Services;
using PersonDetection.Client.Infrastructure.Common;

namespace PersonDetection.Client.Infrastructure.Services;

public class HttpPhotoProcessService(
    CacheHttpClientService httpClient, 
    IInfrastructureConfiguration configuration) : IPhotoProcessService
{
    public async Task<PhotoProcessResultDto> ProcessPhoto(PhotoToProcess photoToProcess, CancellationToken cancellationToken = default)
    {
        return await httpClient.GetAsync<PhotoProcessResultDto>(configuration.PhotoProcessUrl + "photo", cancellationToken);
    }
}
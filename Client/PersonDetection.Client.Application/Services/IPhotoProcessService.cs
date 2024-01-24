using PersonDetection.Client.Application.Dto;
using PersonDetection.Client.Application.Models;

namespace PersonDetection.Client.Application.Services;

public interface IPhotoProcessService
{
    public Task<PhotoProcessResultDto> ProcessPhoto(PhotoToProcess photoToProcess, CancellationToken cancellationToken = default);
}
using PersonDetection.Backend.Application.Common.Models;

namespace PersonDetection.Backend.Application.Services;

public interface IPhotoProcessingService
{
    public Task<string> ProcessPhotoAsync(string photo);
    public Task<Photo> ProcessPhotoTransparentAsync(string photo);
}
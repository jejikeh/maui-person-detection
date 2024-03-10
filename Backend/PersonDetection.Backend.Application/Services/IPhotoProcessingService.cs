using PersonDetection.Backend.Application.Common.Models;

namespace PersonDetection.Backend.Application.Services;

public interface IPhotoProcessingService
{
    public Task<string> ProcessPhotoAsync(string base64Image);
    public void RunInBackground(string photo, OnnxModelType modelType, Func<string, Task> handlePipelineCompleteAsync);
}
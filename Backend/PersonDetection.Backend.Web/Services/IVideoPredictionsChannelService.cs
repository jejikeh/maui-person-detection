using PersonDetection.Backend.Application.Common.Models;

namespace PersonDetection.Backend.Web.Services;

public interface IVideoPredictionsChannelService
{
    public Task StreamPhotoAsync(string connectionId, IAsyncEnumerable<string> photosStream, OnnxModelType modelType);
}
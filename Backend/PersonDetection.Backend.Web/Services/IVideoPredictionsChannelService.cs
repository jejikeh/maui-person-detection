using System.Threading.Channels;
using PersonDetection.Backend.Application.Common.Models;

namespace PersonDetection.Backend.Web.Services;

public interface IVideoPredictionsChannelService
{
    public Task StreamPhotoAsync(IAsyncEnumerable<string> photosStream, OnnxModelType modelType);
}
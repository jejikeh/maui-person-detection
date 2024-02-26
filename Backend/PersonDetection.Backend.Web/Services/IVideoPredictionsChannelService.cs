using System.Threading.Channels;

namespace PersonDetection.Backend.Web.Services;

public interface IVideoPredictionsChannelService
{
    public ChannelReader<string> GetReader();
    public Task StreamTransparentPhotoAsync(string data);
    public IAsyncEnumerable<string> StreamPhotoAsync(IAsyncEnumerable<string> photosStream);
}
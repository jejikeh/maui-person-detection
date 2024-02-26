using System.Threading.Channels;
using PersonDetection.Backend.Application.Services;

namespace PersonDetection.Backend.Web.Services.Implementations;

public class VideoPredictionsChannelService(IPhotoProcessingService _processingService) : IVideoPredictionsChannelService
{
    private readonly Channel<string> _messages = Channel.CreateUnbounded<string>();
    
    public ChannelReader<string> GetReader() => _messages.Reader;
    
    public async Task StreamTransparentPhotoAsync(string data)
    {
        var processPhoto = await _processingService.ProcessPhotoAsync(data);
        
        await _messages.Writer.WriteAsync(processPhoto);
    }

    public IAsyncEnumerable<string> StreamPhotoAsync(IAsyncEnumerable<string> photosStream)
    {
        return _processingService.ProcessPhotosStreamAsync(photosStream);
    }
}
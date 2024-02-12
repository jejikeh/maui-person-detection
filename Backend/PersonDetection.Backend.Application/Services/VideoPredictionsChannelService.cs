using System.Threading.Channels;
using PersonDetection.Backend.Application.Models;

namespace PersonDetection.Backend.Application.Services;

public class VideoPredictionsChannelService(PhotoProcessingService processingService)
{
    private readonly Channel<string> _messages = Channel.CreateUnbounded<string>();
    
    public ChannelReader<string> GetReader() => _messages.Reader;

    public async Task StreamTransparentPhotoAsync(string data)
    {
        var processPhoto = await processingService.ProcessPhotoTransparentAsync(new Photo()
        {
            Content = data
        });
        
        await _messages.Writer.WriteAsync(processPhoto.Content);
    }
}
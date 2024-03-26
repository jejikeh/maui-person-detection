using System.Threading.Channels;
using Microsoft.AspNetCore.SignalR;
using PersonDetection.Backend.Application.Services;
using PersonDetection.Backend.Web.Hubs;

namespace PersonDetection.Backend.Web.Services.Implementations;

public class VideoPredictionsChannelService(
    IPhotoProcessingService _processingService, 
    IHubContext<VideoHub> _hubContext) : IVideoPredictionsChannelService
{
    private static readonly string _sendPhotoMethodName = "SendPhoto";
    private readonly Channel<string> _messages = Channel.CreateUnbounded<string>();
    
    public ChannelReader<string> GetReader() => _messages.Reader;
    
    public async Task StreamTransparentPhotoAsync(string data)
    {
        var processPhoto = await _processingService.ProcessTransparentPhotoAsync(data);
        
        await _messages.Writer.WriteAsync(processPhoto);
    }

    public async Task StreamPhotoAsync(IAsyncEnumerable<string> photosStream)
    {
        await foreach (var photo in photosStream)
        {
            _processingService.RunInBackground(
                photo,
                async processedImage => 
                    await _hubContext.Clients.All.SendAsync(_sendPhotoMethodName, processedImage));
        }
    }
}
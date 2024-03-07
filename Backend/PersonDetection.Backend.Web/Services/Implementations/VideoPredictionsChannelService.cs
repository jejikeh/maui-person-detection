using Microsoft.AspNetCore.SignalR;
using PersonDetection.Backend.Application.Common.Models;
using PersonDetection.Backend.Application.Services;
using PersonDetection.Backend.Web.Hubs;

namespace PersonDetection.Backend.Web.Services.Implementations;

public class VideoPredictionsChannelService(
    IPhotoProcessingService _processingService,
    IHubContext<VideoHub> _hubContext) : IVideoPredictionsChannelService
{
    private static readonly string _sendPhotoMethodName = "ProcessPhotoOutput";

    public async Task StreamPhotoAsync(string connectionId, IAsyncEnumerable<string> photosStream, OnnxModelType modelType)
    {
        await foreach (var photo in photosStream)
        {
            _processingService.RunInBackground(
                photo,
                modelType,
                async processedImage =>
                    await _hubContext.Clients.Client(connectionId).SendAsync(_sendPhotoMethodName, processedImage));
        }
    }
}

using System.Diagnostics;
using System.Threading.Channels;
using Microsoft.AspNetCore.SignalR;
using PersonDetection.Backend.Application.Common.Models;
using PersonDetection.Backend.Web.Services;

namespace PersonDetection.Backend.Web.Hubs;

public class VideoHub(IVideoPredictionsChannelService _predictionsChannelService) : Hub
{
    public async Task ProcessYolo5Photo(IAsyncEnumerable<string> photos)
    { 
        await _predictionsChannelService.StreamPhotoAsync(photos, OnnxModelType.Yolo5);
    }
    
    public async Task ProcessYolo8Photo(IAsyncEnumerable<string> photos)
    { 
        await _predictionsChannelService.StreamPhotoAsync(photos, OnnxModelType.Yolo8);
    }
} 
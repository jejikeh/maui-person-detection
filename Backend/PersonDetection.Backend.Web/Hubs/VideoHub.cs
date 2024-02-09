using System.Collections.Concurrent;
using System.Runtime.CompilerServices;
using System.Threading.Channels;
using Microsoft.AspNetCore.SignalR;
using PersonDetection.Backend.Web.Services;
using PersonDetection.ImageProcessing;
using PersonDetection.ImageProcessing.Model;

namespace PersonDetection.Backend.Web.Hubs;

public class VideoHub(VideoPredictionsChannelService predictionsChannelService) : Hub
{
    public async Task<ChannelReader<string>> ReceiveVideoData(string data, CancellationToken cancellationToken)
    {
        _ = predictionsChannelService.WriteItemsAsync(data, cancellationToken);
        
        return predictionsChannelService.GetReader();
    }

    public async Task SendVideoData(string data)
    {
        await Clients.All.SendAsync("ReceiveVideoData", data);
    }
}
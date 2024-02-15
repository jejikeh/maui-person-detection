using System.Diagnostics;
using System.Threading.Channels;
using Microsoft.AspNetCore.SignalR;
using PersonDetection.Backend.Web.Services;

namespace PersonDetection.Backend.Web.Hubs;

public class VideoHub(IVideoPredictionsChannelService _predictionsChannelService) : Hub
{
    private readonly Stopwatch _stopwatch = new();
    
    public async Task<ChannelReader<string>> ReceiveVideoData(string data)
    {
        _stopwatch.Start();
       
        _ = _predictionsChannelService.StreamTransparentPhotoAsync(data);
       
        _stopwatch.Stop();
        await SendModelPerformance(_stopwatch.ElapsedMilliseconds.ToString());
        _stopwatch.Reset();
        
        return _predictionsChannelService.GetReader();
    }

    public async Task SendModelPerformance(string data)
    {
        await Clients.All.SendAsync("SendModelPerformance", data);
    }
}
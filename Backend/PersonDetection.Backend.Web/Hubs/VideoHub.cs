using System.Diagnostics;
using System.Threading.Channels;
using Microsoft.AspNetCore.SignalR;
using PersonDetection.Backend.Application.Services;

namespace PersonDetection.Backend.Web.Hubs;

public class VideoHub(VideoPredictionsChannelService predictionsChannelService) : Hub
{
    private readonly Stopwatch _stopwatch = new();
    
    public async Task<ChannelReader<string>> ReceiveVideoData(string data)
    {
        _stopwatch.Start();
       
        _ = predictionsChannelService.StreamTransparentPhotoAsync(data);
       
        _stopwatch.Stop();
        await SendModelPerformance(_stopwatch.ElapsedMilliseconds.ToString());
        _stopwatch.Reset();
        
        return predictionsChannelService.GetReader();
    }

    public async Task SendModelPerformance(string data)
    {
        await Clients.All.SendAsync("SendModelPerformance", data);
    }
}
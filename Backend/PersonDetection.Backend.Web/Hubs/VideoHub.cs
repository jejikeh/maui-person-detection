using System.Text.Json.Serialization;
using Microsoft.AspNetCore.SignalR;
using PersonDetection.ImageProcessing;

namespace PersonDetection.Backend.Web.Hubs;

[method: JsonConstructor]
public class VideoData(int index, string data)
{
    public int Index { get; set; } = index;
    public string Data { get; set; } = data;
}

public class VideoHub(YoloImageProcessing imageProcessing) : Hub
{
    public override Task OnConnectedAsync()
    {
        Console.WriteLine("Connected: " + Context.ConnectionId);
        
        return base.OnConnectedAsync();
    }

    public async Task SendVideoData(VideoData data)
    {
        await Clients.All.SendAsync("ReceiveVideoData", new VideoData(0, await imageProcessing.PredictAsync(data.Data)));
    }
}
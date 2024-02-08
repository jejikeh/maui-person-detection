using System.Threading.Channels;
using PersonDetection.ImageProcessing;
using PersonDetection.ImageProcessing.Model;

namespace PersonDetection.Backend.Web.Services;

public class VideoPredictionsChannelService(YoloImageProcessing yoloImageProcessing)
{
    private readonly Channel<string> _messages = Channel.CreateUnbounded<string>();
    private List<YoloPrediction> _cachedPredictions = new List<YoloPrediction>();
    private string _cachedData = string.Empty;
    public ChannelReader<string> GetReader() => _messages.Reader;

    public async Task WriteItemsAsync(string data, CancellationToken cancellationToken)
    {
        _cachedPredictions = await yoloImageProcessing.CalculatePredictionsAsync(data);
        _cachedData = await yoloImageProcessing.DrawPredictionsAsync(_cachedPredictions);

        await _messages.Writer.WriteAsync(_cachedData, cancellationToken);
    }
}
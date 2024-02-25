using System.Collections.Concurrent;
using System.Threading.Channels;
using Neural.Onnx.Models.Yolo5.Tasks.BoxPredictionsToImage;
using Neural.Onnx.Models.Yolo5.Tasks.ImageToBoxPredictions;
using SixLabors.ImageSharp;

namespace Neural.Onnx.Pipelines;

public class Yolo5ImageBackgroundPipeline : Yolo5ImagePipeline
{
    private readonly Channel<BoxPredictionsToImageTasks> _channel = Channel.CreateUnbounded<BoxPredictionsToImageTasks>();
    
    public async IAsyncEnumerable<BoxPredictionsToImageTasks> RunAsync(ImageToBoxPredictionsTask task)
    {
        if (!ClustersInitialized())
        {
            yield break;
        }
        
        var model = await Yolo5Cluster!.RunInBackgroundAsync(task);

        model.OnModelTaskComplete += predictionsTask =>
        {
            var predictions = new BoxPredictionsToImageTasks(predictionsTask as ImageToBoxPredictionsTask);
            
            var image = ImageBoxPainterCluster!.RunAsync(predictions);
            
            _channel.Writer.TryWrite(predictions);
        };

        while (await _channel.Reader.WaitToReadAsync())
        {
            if (_channel.Reader.TryRead(out var predictions))
            {
                yield return predictions;
            } 
        }
    }
}
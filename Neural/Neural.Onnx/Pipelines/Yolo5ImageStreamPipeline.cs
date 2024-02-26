using System.Collections.Concurrent;
using Neural.Onnx.Models.Yolo5.Tasks.BoxPredictionsToImage;
using Neural.Onnx.Models.Yolo5.Tasks.ImageToBoxPredictions;
using SixLabors.ImageSharp;

namespace Neural.Onnx.Pipelines;

public class Yolo5ImageStreamPipeline : Yolo5ImagePipeline
{
    private readonly ConcurrentQueue<string> _imageStack = new ConcurrentQueue<string>();
    
    public async IAsyncEnumerable<string> RunAsync(IAsyncEnumerable<string> task)
    {
        if (!ClustersInitialized())
        {
            yield break;
        }
        
        await foreach (var input in task)
        {
                var prediction = await Yolo5Cluster!.RunAsync(new ImageToBoxPredictionsTask(input));
                var image = await ImageBoxPainterCluster!.RunAsync(new BoxPredictionsToImageTasks(prediction));

                if (image?.TypedOutput.Image is null)
                {
                    continue;
                }

                yield return await ConvertImageToStringAsync(image.TypedOutput.Image);
        }
    }
    
    private static async Task<string> ConvertImageToStringAsync(Image image)
    {
        var stream = new MemoryStream();
        
        await image.SaveAsPngAsync(stream);
        
        return Convert.ToBase64String(stream.ToArray());
    }
}
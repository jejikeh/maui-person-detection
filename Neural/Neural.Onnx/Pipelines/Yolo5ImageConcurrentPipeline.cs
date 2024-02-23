using System.Collections.Concurrent;
using Neural.Onnx.Tasks.BoxPredictionsToImage;
using Neural.Onnx.Tasks.ImageToBoxPredictions;

namespace Neural.Onnx.Pipelines;

public class Yolo5ImageConcurrentPipeline : Yolo5ImagePipeline
{
    public async Task<BoxPredictionsToImageTasks[]> RunAsync(IEnumerable<ImageToBoxPredictionsTask> task)
    {
        if (!ClustersInitialized())
        {
            return [];
        }

        var images = new ConcurrentBag<BoxPredictionsToImageTasks>();

        await Yolo5Cluster!.RunHandleAsync(task, async yolo5Output =>
        {
            var image = await ImageBoxPainterCluster!.RunAsync(new BoxPredictionsToImageTasks(yolo5Output));

            if (image is not null)
            {
                images.Add(image);
            }
        });
        
        return images.ToArray();
    }
}
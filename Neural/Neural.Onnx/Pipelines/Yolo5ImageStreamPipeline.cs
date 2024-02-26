using System.Collections.Concurrent;
using Neural.Onnx.Models.Yolo5.Tasks.BoxPredictionsToImage;
using SixLabors.ImageSharp;

namespace Neural.Onnx.Pipelines;

public class Yolo5ImageStreamPipeline : Yolo5ImagePipeline
{
    private readonly ConcurrentQueue<string> _imageStack = new ConcurrentQueue<string>();

    public void RunInBackground(string task, Func<string, Task> handlePipelineCompleteAsync)
    {
        if (!ClustersInitialized())
        {
            return;
        }
        
        Yolo5Cluster!.RunInBackground(task, async prediction =>
        {
            var image = await ImageBoxPainterCluster!.RunAsync(new BoxPredictionsToImageTasks(prediction));

            if (image?.TypedOutput.Image is null)
            {
                return;
            }

            var outputContent = await ConvertImageToStringAsync(image.TypedOutput.Image);

            await handlePipelineCompleteAsync(outputContent);
        });
    }
    
    private static async Task<string> ConvertImageToStringAsync(Image image)
    {
        var stream = new MemoryStream();
        
        await image.SaveAsPngAsync(stream);
        
        return Convert.ToBase64String(stream.ToArray());
    }
}
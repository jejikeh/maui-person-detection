using Neural.Onnx.Models.Yolo8.SegmentationsPredictionsToImage;
using SixLabors.ImageSharp;

namespace Neural.Onnx.Pipelines.Yolo8;

public class Yolo8ImageStreamPipeline : Yolo8ImagePipeline
{
    public void RunInBackground(string task, Func<string, Task> handlePipelineCompleteAsync)
    {
        if (!ClustersInitialized())
        {
            return;
        }
        
        Yolo8Cluster!.RunInBackground(task, async prediction =>
        {
            var image = await ImageBoxPainterCluster!.RunAsync(new SegmentationPredictionsToImageTask(prediction));

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
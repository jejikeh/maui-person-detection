using Neural.Onnx.Models.Yolo5.Tasks.BoxPredictionsToImage;
using Neural.Onnx.Models.Yolo5.Tasks.ImageToBoxPredictions;

namespace Neural.Onnx.Pipelines;

public class Yolo5ImagePlainPipeline : Yolo5ImagePipeline
{
    public async Task<BoxPredictionsToImageTasks?> RunAsync(ImageToBoxPredictionsTask task)
    {
        if (!ClustersInitialized())
        {
            return null;
        }

        var predictions = await Yolo5Cluster!.RunAsync(task);
        
        if (predictions is null)
        {
            return null;
        }
        
        var image = await ImageBoxPainterCluster!.RunAsync(new BoxPredictionsToImageTasks(predictions));
        
        return image;
    }
}
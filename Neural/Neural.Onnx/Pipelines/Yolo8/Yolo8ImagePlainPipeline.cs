using Neural.Onnx.Models.Yolo5.Tasks.BoxPredictionsToImage;
using Neural.Onnx.Models.Yolo5.Tasks.ImageToBoxPredictions;
using Neural.Onnx.Models.Yolo8.SegmentationsPredictionsToImage;
using Neural.Onnx.Pipelines.Yolo5;
using Neural.Onnx.Tasks.ImageToSegmentation;

namespace Neural.Onnx.Pipelines.Yolo8;

public class Yolo8ImagePlainPipeline : Yolo8ImagePipeline
{
    public async Task<SegmentationPredictionsToImageTask?> RunAsync(ImageToSegmentationTask task)
    {
        if (!ClustersInitialized())
        {
            return null;
        }

        var predictions = await Yolo8Cluster!.RunAsync(task);
        
        if (predictions is null)
        {
            return null;
        }
        
        var image = await ImageBoxPainterCluster!.RunAsync(new SegmentationPredictionsToImageTask(predictions));
        
        return image;
    }
}
using Neural.Defaults.Models;
using Neural.Onnx.Common.Dependencies;
using Neural.Onnx.Models.Yolo5.Tasks.BoxPredictionsToImage;
using Neural.Onnx.Models.Yolo8.SegmentationsPredictionsToImage;
using Neural.Onnx.Services;

namespace Neural.Onnx.Models.ImageBoxPainter;

public class ImageSegmentationPainterModel : Model<SegmentationPredictionsToImageTask, ImageBoxPainterDependencies>
{
    private IImageBoxPainterService? ImageBoxPainterService => 
        DependencyContainer?.ImageBoxPainterService; 
    
    protected override Task<SegmentationPredictionsToImageTask> ProcessAsync(SegmentationPredictionsToImageTask task)
    {
        var inputImage = task.TypedInput.InputImage;
        var predictions = task.TypedInput.Predictions;
        
        ImageBoxPainterService?.PaintPredictions(inputImage, predictions);
        
        task.SetOutput(this, inputImage);
        
        return Task.FromResult(task);
    }
}
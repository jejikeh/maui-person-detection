using Neural.Defaults.Models;
using Neural.Onnx.Common.Dependencies;
using Neural.Onnx.Models.Yolo5.Tasks.BoxPredictionsToImage;
using Neural.Onnx.Services;

namespace Neural.Onnx.Models.ImageBoxPainter;

public class ImageBoxPainterModel : Model<BoxPredictionsToImageTask, ImageBoxPainterDependencies>
{
    private IImageBoxPainterService? ImageBoxPainterService => 
        DependencyContainer?.ImageBoxPainterService; 
    
    protected override Task<BoxPredictionsToImageTask> ProcessAsync(BoxPredictionsToImageTask task)
    {
        var inputImage = task.TypedInput.InputImage;
        var predictions = task.TypedInput.Predictions;
        
        ImageBoxPainterService?.PaintPredictions(inputImage, predictions);
        
        task.SetOutput(this, inputImage);
        
        return Task.FromResult(task);
    }
}
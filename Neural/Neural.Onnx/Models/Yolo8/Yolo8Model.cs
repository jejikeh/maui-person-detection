using Neural.Onnx.Tasks.ImageToSegmentation;

namespace Neural.Onnx.Models.Yolo8;

public class Yolo8Model : OnnxModel<ImageToSegmentationTask>
{
    protected override Task<ImageToSegmentationTask> ProcessAsync(ImageToSegmentationTask task)
    {
        var namedOnnxValues = task
            .TypedInput
            .GetNamedOnnxValues();
        
        using var result = InferenceSession?.Run(namedOnnxValues);
        
        if (result == null)
        {
            return Task.FromResult(task);
        }
        
        task.SetOutput(this, result);
        
        return Task.FromResult(task);
    }
}
using Neural.Onnx.Tasks.ImageToSegmentation;

namespace Neural.Onnx.Models.Yolo8;

public class Yolo8Model : OnnxModel<ImageToSegmentationTask>
{
    protected override Task<ImageToSegmentationTask> ProcessAsync(ImageToSegmentationTask task)
    {
        var namedOnnxValues = task
            .Yolo8ImageInput()
            .GetNamedOnnxValues();
        
        using var result = InferenceSession?.Run(namedOnnxValues);
        
        if (result == null)
        {
            throw new NullReferenceException(nameof(result));
        }
        
        task.SetOutput(this, result);
        
        return Task.FromResult(task);
    }
}
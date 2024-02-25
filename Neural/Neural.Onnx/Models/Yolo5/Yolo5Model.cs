using Neural.Onnx.Models.Yolo5.Tasks.ImageToBoxPredictions;

namespace Neural.Onnx.Models.Yolo5;

public class Yolo5Model : OnnxModel<ImageToBoxPredictionsTask>
{
    protected override Task<ImageToBoxPredictionsTask> ProcessAsync(ImageToBoxPredictionsTask task)
    {
        var namedOnnxValues = task
            .TypedInput
            .GetNamedOnnxValues();
        
        using var result = InferenceSession?.Run(namedOnnxValues);
        
        if (result is null)
        {
            return Task.FromResult(task);
        }
        
        task.SetOutput(this, result);
        
        return Task.FromResult(task);
    }
}
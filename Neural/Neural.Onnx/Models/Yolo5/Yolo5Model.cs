using Neural.Core.Models;
using Neural.Onnx.Tasks.ImageToBoxPredictions;

namespace Neural.Onnx.Models.Yolo5;

public class Yolo5Model : OnnxModel<ImageToBoxPredictionsTask>
{
    public override Task<ImageToBoxPredictionsTask> RunAsync(ImageToBoxPredictionsTask task)
    {
        if (InferenceSession is null)
        {
            throw new NullReferenceException(nameof(InferenceSession));
        }
        
        Status = ModelStatus.Active;

        var namedOnnxValues = task
            .Yolo5ImageInput()
            .GetNamedOnnxValues();
        
        var result = InferenceSession.Run(namedOnnxValues);
        
        task.SetOutput(this, result);
        
        Status = ModelStatus.Inactive;
        
        return Task.FromResult(task);
    }
}
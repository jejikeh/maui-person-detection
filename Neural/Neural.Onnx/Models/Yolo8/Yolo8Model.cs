using Neural.Core.Models;
using Neural.Onnx.Tasks.ImageToSegmentation;

namespace Neural.Onnx.Models.Yolo8;

public class Yolo8Model : OnnxModel<ImageToSegmentationTask>
{
    public override Task<ImageToSegmentationTask> RunAsync(ImageToSegmentationTask task)
    {
        if (InferenceSession is null)
        {
            throw new NullReferenceException(nameof(InferenceSession));
        }
        
        Status = ModelStatus.Active;
        
        var namedOnnxValues = task
            .Yolo8ImageInput()
            .GetNamedOnnxValues();
        
        using var result = InferenceSession.Run(namedOnnxValues);
        
        task.SetOutput(this, result);

        Status = ModelStatus.Inactive;
        
        return Task.FromResult(task);
    }
}
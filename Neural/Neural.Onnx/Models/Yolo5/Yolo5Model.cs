using Neural.Core.Models;
using Neural.Onnx.Tasks.ImageToBoxPredictions;

namespace Neural.Onnx.Models.Yolo5;

public class Yolo5Model : OnnxModel<ImageToBoxPredictionsYolo5Task>
{
    public override Task<ImageToBoxPredictionsYolo5Task> RunAsync(ImageToBoxPredictionsYolo5Task yolo5Task)
    {
        if (InferenceSession is null)
        {
            throw new NullReferenceException(nameof(InferenceSession));
        }
        
        Status = ModelStatus.Active;

        var namedOnnxValues = yolo5Task
            .Yolo5ImageInput()
            .GetNamedOnnxValues();
        
        var result = InferenceSession.Run(namedOnnxValues);
        
        yolo5Task.SetOutput(this, result);
        
        Status = ModelStatus.Inactive;
        
        return Task.FromResult(yolo5Task);
    }
}
using Microsoft.ML.OnnxRuntime;
using Microsoft.ML.OnnxRuntime.Tensors;
using Neural.Core.Models;
using Neural.Onnx.Common;
using Neural.Onnx.Models.Yolo5.Specifications;
using Neural.Onnx.Models.Yolo5.Tensors;
using SixLabors.ImageSharp;

namespace Neural.Onnx.Models.Yolo5.Tasks.ImageToBoxPredictions;

public class Yolo5BoxPredictionsOutput : IModelOutput
{
    public List<Yolo5Prediction>? Predictions { get; private set; }
    
    public void Set(object value)
    {
        if (value is not IDisposableReadOnlyCollection<DisposableNamedOnnxValue> onnxValues)
        {
            throw new ArgumentException(nameof(value));
        }
        
        Predictions = new Yolo5OutputTensor(onnxValues).ExtractFilterOverlappingPredictions();
    }
}
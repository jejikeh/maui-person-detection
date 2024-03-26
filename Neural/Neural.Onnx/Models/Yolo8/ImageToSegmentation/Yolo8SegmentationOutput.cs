using Microsoft.ML.OnnxRuntime;
using Neural.Core.Models;
using Neural.Onnx.Models.Yolo8.Specifications;
using Neural.Onnx.Models.Yolo8.Tensors;

namespace Neural.Onnx.Models.Yolo8.ImageToSegmentation;

public class Yolo8SegmentationOutput : IModelOutput
{
    public IEnumerable<SegmentationBoundBox>? Predictions { get; set; }
    
    public void Set(object value)
    {
        if (value is not IDisposableReadOnlyCollection<DisposableNamedOnnxValue> onnxValues)
        {
            throw new ArgumentException(nameof(value));
        }

        Predictions = new Yolo8OutputTensor(onnxValues).ExtractSegmentationBoundBoxes();
    }
}
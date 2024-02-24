using Microsoft.ML.OnnxRuntime;
using Neural.Core.Models;
using Neural.Onnx.Models.Yolo8.Specifications;

namespace Neural.Onnx.Tasks.ImageToSegmentation;

public class Yolo8SegmentationOutput : IModelOutput
{
    public SegmentationBoundBox[]? Predictions { get; set; }
    
    public void Set(object value)
    {
        if (value is not IDisposableReadOnlyCollection<DisposableNamedOnnxValue> onnxValues)
        {
            throw new ArgumentException(nameof(value));
        }

        var outputTensor = new List<NamedOnnxValue>(onnxValues);

        var boxesTensor = outputTensor[Yolo8OutputSpecification.BoxesTensorDimensionLayer].AsTensor<float>();
        var segmentationsTensor = outputTensor[Yolo8OutputSpecification.SegmentationLayer].AsTensor<float>();

        var indexedBoundingBoxes = boxesTensor
            .ExtractIndexedBoundingBoxes()
            ;
        
        
        
    }
}
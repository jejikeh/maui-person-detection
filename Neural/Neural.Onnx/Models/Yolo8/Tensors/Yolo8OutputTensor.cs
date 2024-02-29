using Microsoft.ML.OnnxRuntime;
using Microsoft.ML.OnnxRuntime.Tensors;
using Neural.Onnx.Models.Yolo8.Specifications;

namespace Neural.Onnx.Models.Yolo8.Tensors;

public class Yolo8OutputTensor
{
    private readonly Tensor<float> _boundTensor;
    private readonly Tensor<float> _segmentationTensor;

    public Yolo8OutputTensor(IDisposableReadOnlyCollection<DisposableNamedOnnxValue> onnxValues)
    {
        var modelOnnxValues = new List<NamedOnnxValue>(onnxValues);
        
        _boundTensor = modelOnnxValues[Yolo8OutputSpecification.BoxesTensorDimensionLayer].AsTensor<float>();
        _segmentationTensor = modelOnnxValues[Yolo8OutputSpecification.SegmentationLayer].AsTensor<float>();
    }

    public IEnumerable<SegmentationBoundBox> ExtractSegmentationBoundBoxes()
    {
        var indexedBoundBoxes = ExtractIndexedBoundBoxes();
        
        return _segmentationTensor.ToSegmentationBoundBoxes(
            _boundTensor,
            indexedBoundBoxes);
    }

    private List<IndexedBoundingBox> ExtractIndexedBoundBoxes()
    {
        return _boundTensor.ExtractIndexedBoundingBoxes();
    }
}
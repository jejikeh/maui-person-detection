using System.Collections.Concurrent;
using Microsoft.ML.OnnxRuntime;
using Microsoft.ML.OnnxRuntime.Tensors;
using Neural.Onnx.Common;
using Neural.Onnx.Models.Yolo5.Specifications;
using SixLabors.ImageSharp;

namespace Neural.Onnx.Models.Yolo5.Tensors;

public class Yolo5OutputTensor
{
    private readonly DenseTensor<float> _outputTensors;
    
    public Yolo5OutputTensor(IDisposableReadOnlyCollection<DisposableNamedOnnxValue> onnxValues)
    {
        _outputTensors = Yolo5Specification.Outputs
            .Select(item => onnxValues.First(x => x.Name == item).Value as DenseTensor<float>)
            .First()!;
    }

    private List<Yolo5Prediction> ExtractYoloPredictions()
    {
        var result = new ConcurrentBag<Yolo5Prediction>();
        
        // We do not calculate Scaling Factors and Paddings, since input image is already scaled in Yolo5ImageInput

        var outputsEntryCount = (int)_outputTensors.Length / Yolo5OutputSpecification.Dimensions;

        Parallel.For(0, outputsEntryCount, prediction =>
        {
            if (_outputTensors.IsConfidenceThresholdExceeded(prediction))
            {
                return;
            }

            _outputTensors.ScaleTensorLayersAccordingToConfidence(prediction);
            
            _outputTensors.ConvertToYoloPredictions(prediction, result.Add);
        });
        
        return result.ToList();
    }
    
    public IEnumerable<Yolo5Prediction> ExtractFilterOverlappingPredictions()
    {
        var predictions = ExtractYoloPredictions();
        var result = new HashSet<Yolo5Prediction>();

        foreach (var prediction in predictions.OrderByDescending(yolo5Prediction => yolo5Prediction.Score))
        {
            if (!result.Any(yolo5Prediction =>
                    RectanglesOverlapExcessively(yolo5Prediction.Rectangle, prediction.Rectangle)))
            {
                result.Add(prediction);
            }
        }

        return result;
    }
    
    private static bool RectanglesOverlapExcessively(RectangleF rectangle1, RectangleF rectangle2)
    {
        if (!rectangle1.IntersectsWith(rectangle2))
        {
            return false;
        }
        
        var overlapRectangle = RectangleF.Intersect(rectangle1, rectangle2);

        return overlapRectangle.IsOverlappingAboveThreshold(
            rectangle1,
            rectangle2,
            Yolo5OutputSpecification.OverlapThreshold
        );
    }
}
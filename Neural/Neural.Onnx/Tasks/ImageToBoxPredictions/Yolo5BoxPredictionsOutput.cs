using Microsoft.ML.OnnxRuntime;
using Microsoft.ML.OnnxRuntime.Tensors;
using Neural.Core.Models;
using Neural.Onnx.Common;
using Neural.Onnx.Models.Yolo5.Specifications;
using SixLabors.ImageSharp;

namespace Neural.Onnx.Tasks.ImageToBoxPredictions;

public class Yolo5BoxPredictionsOutput : IModelOutput
{
    public List<Yolo5Prediction>? Predictions { get; set; }
    
    public void Set(object value)
    {
        if (value is not IDisposableReadOnlyCollection<DisposableNamedOnnxValue> onnxValues)
        {
            throw new ArgumentException(nameof(value));
        }
        
        var outputTensor = ExtractOutputTensors(onnxValues).First();

        Predictions = FilterOverlappingPredictions(outputTensor.ExtractYoloPredictions());
    }

    private static DenseTensor<float>[] ExtractOutputTensors(IDisposableReadOnlyCollection<DisposableNamedOnnxValue> onnxValues)
    {
        return Yolo5Specification.Outputs
            .Select(item => onnxValues.First(x => x.Name == item).Value as DenseTensor<float>)
            .ToArray() ?? throw new ArgumentNullException(nameof(onnxValues));
    }

    private static List<Yolo5Prediction> FilterOverlappingPredictions(List<Yolo5Prediction> predictions)
    {
        var result = new List<Yolo5Prediction>(predictions);

        foreach (var prediction in predictions)
        {
            foreach (var otherPrediction in result.ToList().Where(yolo5Prediction => yolo5Prediction != prediction))
            {
                var rectangle1 = prediction.Rectangle;
                var rectangle2 = otherPrediction.Rectangle;

                var overlapRectangle = RectangleF.Intersect(rectangle1, rectangle2);
                
                if (!overlapRectangle.IsOverlappingAboveThreshold(
                        rectangle1, 
                        rectangle2,
                        Yolo5OutputSpecification.OverlapThreshold))
                {
                    continue;
                }
                
                if (prediction.Score >= otherPrediction.Score)
                {
                    result.Remove(otherPrediction);
                }
            }
        }

        return result;
    }
}
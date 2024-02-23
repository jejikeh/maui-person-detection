using Microsoft.ML.OnnxRuntime.Tensors;
using Neural.Onnx.Tasks.ImageToSegmentation;
using SixLabors.ImageSharp;

namespace Neural.Onnx.Models.Yolo8.Specifications;

public static class Yolo8OutputSpecification
{
    public const int BoxesTensorDimensionLayer = 0;
    public const int SegmentationLayer = 1;
    public const int BoxesLayer = 2;

    public const int ConfidenceOffsetFromClass = 4;
    
    public static float ConfidenceThreshold = 0.2f;

    private const int _offsetBetweenSegmentations = 4;
    
    public static SegmentationBoundBox[] ExtractSegmentationBoundBoxes(
        this DenseTensor<float> outputTensor,
        Tensor<float> maskTensor)
    {
        var maskChannelCount = outputTensor.GetSegmentationChannelCount();

        Parallel.For(0, maskChannelCount, mask =>
        {
            for (var classIndex = 0; classIndex < Yolo8Specification.Classes.Length; classIndex++)
            {
                if (outputTensor.IsConfidenceThresholdExceeded(classIndex, mask))
                {
                    continue;
                }
                
                
            }
        });
    }
    
    private static RectangleF ExtractBoundingBox(this Tensor<float> tensor, int prediction)
    {
        var xMin = tensor[BatchSize, prediction, _xLayer] - tensor[BatchSize, prediction, _widthLayer] / 2;
        var yMin = tensor[BatchSize, prediction, _yLayer] - tensor[BatchSize, prediction, _heightLayer] / 2;
        
        var xMax = tensor[BatchSize, prediction, _xLayer] + tensor[BatchSize, prediction, _widthLayer] / 2;
        var yMax = tensor[BatchSize, prediction, _yLayer] + tensor[BatchSize, prediction, _heightLayer] / 2;
        
        return new RectangleF(xMin, yMin, xMax - xMin, yMax - yMin);
    }
    
    public static float GetConfidence(this Tensor<float> outputTensor, int boxes, int mask)
    {
        return outputTensor[BoxesTensorDimensionLayer, boxes + ConfidenceOffsetFromClass, mask];
    }
    
    public static bool IsConfidenceThresholdExceeded(this Tensor<float> outputTensor, int boxes, int mask)
    {
        return outputTensor.GetConfidence(boxes, mask) <= ConfidenceThreshold;
    }

    private static int GetSegmentationChannelCount(this Tensor<float> outputTensor)
    {
        return outputTensor.Dimensions[SegmentationLayer] - _offsetBetweenSegmentations - Yolo8Specification.Classes.Length;
    }
}
using System.Collections.Concurrent;
using Microsoft.ML.OnnxRuntime.Tensors;
using Neural.Onnx.Common;
using SixLabors.ImageSharp;

namespace Neural.Onnx.Models.Yolo8.Specifications;

public static class Yolo8OutputSpecification
{
    public const int BoxesTensorDimensionLayer = 0;
    public const int SegmentationLayer = 1;
    public const int Boxes = 2;
    public const int BoxesWidthLayer = 3;

    public const int ConfidenceOffsetFromClass = 4;
    
    public static float ConfidenceThreshold = 0.2f;
    public static float OverlapThreshold { get; set; } = 0.5f;
    
    public static float ModelQuality { get; set; } = 2f;

    private const int _xLayer = 0;
    private const int _yLayer = 1;
    private const int _widthLayer = 2;
    private const int _heightLayer = 3;
    
    public static List<IndexedBoundingBox> ExtractIndexedBoundingBoxes(this Tensor<float> outputTensor)
    {
        var boxesCount = outputTensor.Dimensions[Boxes];
        var boxes = new ConcurrentBag<IndexedBoundingBox>();

        Parallel.For(0, boxesCount, box =>
        {
            Parallel.For(0, Yolo8Specification.Classes.Length, classIndex =>
            {
                if (outputTensor.IsConfidenceThresholdExceeded(classIndex, box))
                {
                    return;
                }

                var bounds = outputTensor.ExtractBoundingBox(classIndex);

                var name = Yolo8Specification.Classes[classIndex];

                if (bounds.IsEmpty)
                {
                    return;
                }

                boxes.Add(new IndexedBoundingBox
                {
                    Index = box,
                    Class = name.DisplayName(),
                    Bounds = bounds,
                    Confidence = outputTensor.GetConfidence(box, classIndex),
                });
            });
        });
        
        return boxes.ToList().FilterOverlappingBoxes();
    }
    
    // @Cleanup: Refactor this. This is a very big function.
    private static List<IndexedBoundingBox> FilterOverlappingBoxes(this IReadOnlyList<IndexedBoundingBox> boxes)
    {
        var activeBoxes = new HashSet<int>(Enumerable.Range(0, boxes.Count));

        var selected = new List<IndexedBoundingBox>();
        
        while(activeBoxes.Count != 0)
        {
            var currentBoxIndex = activeBoxes.First();
            
            activeBoxes.Remove(currentBoxIndex);
            
            var currentBox = boxes[currentBoxIndex];
            selected.Add(currentBox);

            foreach (var otherBoxIndex in activeBoxes)
            {
                var otherBox = boxes[otherBoxIndex];

                // @Cleanup: Recheck this expression
                if (currentBox.Bounds.IsOverlappingAboveThreshold(otherBox.Bounds, OverlapThreshold))
                {
                    activeBoxes.Remove(otherBoxIndex);
                }
            }
        }
        
        return selected;
    }
    
    private static Rectangle ExtractBoundingBox(this Tensor<float> tensor, int prediction)
    {
        var x = tensor[BoxesTensorDimensionLayer, _xLayer, prediction];
        var y = tensor[BoxesTensorDimensionLayer, _yLayer, prediction];
        var width = tensor[BoxesTensorDimensionLayer, _widthLayer, prediction];
        var height = tensor[BoxesTensorDimensionLayer, _heightLayer, prediction];
        
        var xMin = (int)(x - width / 2);
        var yMin = (int)(y - height / 2);
        var xMax = (int)(x + width / 2);
        var yMax = (int)(y + height / 2);
        
        return Rectangle.FromLTRB(xMin, yMin, xMax, yMax);
    }
    
    public static float GetConfidence(this Tensor<float> outputTensor, int boxes, int mask)
    {
        return outputTensor[BoxesTensorDimensionLayer, boxes + ConfidenceOffsetFromClass, mask];
    }
    
    public static bool IsConfidenceThresholdExceeded(this Tensor<float> outputTensor, int classIndex, int boxIndex)
    {
        return outputTensor.GetConfidence(classIndex, boxIndex) <= ConfidenceThreshold;
    }

    private static int GetSegmentationChannelCount(this Tensor<float> outputTensor)
    {
        return outputTensor.Dimensions[SegmentationLayer] - ConfidenceOffsetFromClass - Yolo8Specification.Classes.Length;
    }
    
    // public static SegmentationBoundBox[] ToSegmentationBoundBoxes(this Tensor<float> outputTensor, IndexedBoundingBox[] boxes)
    // {
    //     var segmentationBoundBox = new SegmentationBoundBox[boxes.Length];
    //
    //     var segmentationChannelCount = outputTensor.GetSegmentationChannelCount();
    //
    //     foreach (var box in boxes)
    //     {
    //         var maskWeights = outputTensor.ExtractMaskWeights(
    //             box.Index,
    //             segmentationChannelCount,
    //             Yolo8Specification.Classes.Length + ConfidenceOffsetFromClass);
    //         
    //         var mask = 
    //     }
    // }
    //
    // public static float[] ExtractMaskWeights(this Tensor<float> output, int boxIndex, int maskChannelCount, int maskWeightOffset)
    // {
    //     var maskWeights = new float[maskChannelCount];
    //
    //     for (var maskChannel = 0; maskChannel < maskChannelCount; maskChannel++)
    //     {
    //         maskWeights[maskChannel] = output[BoxesTensorDimensionLayer, maskWeightOffset + maskChannel, boxIndex];
    //     }
    //     
    //     return maskWeights;
    // }
    //
    // private static SegmentationMask ExtractMask(
    //     this Tensor<float> output, 
    //     IReadOnlyList<float> maskWeights,
    //     Rectangle maskBound)
    // {
    //     var maskChannels = output.Dimensions[SegmentationLayer] / ModelQuality;
    //     var maskHeight = output.Dimensions[Boxes];
    //     var maskWidth = output.Dimensions[Layer];
    // }
}
using System.Collections.Concurrent;
using Microsoft.ML.OnnxRuntime.Tensors;
using Neural.Onnx.Common;
using Neural.Onnx.Models.Yolo8.Specifications;
using SixLabors.ImageSharp;

namespace Neural.Onnx.Models.Yolo8.Tensors;

public static class Yolo8TensorExtensions
{
    public static List<IndexedBoundingBox> ExtractIndexedBoundingBoxes(this Tensor<float> outputTensor)
    {
        var boxesCount = outputTensor.Dimensions[Yolo8OutputSpecification.Boxes];
        var boxes = new ConcurrentBag<IndexedBoundingBox>();

        Parallel.For(0, boxesCount, box =>
        {
            for (var classIndex = 0; classIndex < Yolo8Specification.Classes.Length; classIndex++) 
            {
                if (outputTensor.IsConfidenceThresholdExceeded(classIndex, box))
                {
                    continue;
                }

                var bounds = outputTensor.ExtractBoundingBox(box);

                var name = Yolo8Specification.Classes[classIndex];

                if (bounds.IsEmpty)
                {
                    return;
                }

                boxes.Add(new IndexedBoundingBox
                {
                    Index = box,
                    Class = name,
                    Bounds = bounds,
                    Confidence = outputTensor.GetConfidence(classIndex, box),
                });
            }
        });
        
        return IndexedBoundingBox.FilterOverlappingBoxes(boxes.ToList());
    }
    
    private static Rectangle ExtractBoundingBox(this Tensor<float> tensor, int prediction)
    {
        var boundBox = CalculateRectangleFromTensor(tensor, prediction);

        return boundBox.CenterRectangle();
    }

    private static Rectangle CalculateRectangleFromTensor(this Tensor<float> tensor, int prediction)
    {
        var x = tensor[Yolo8OutputSpecification.BoxesTensorDimensionLayer, Yolo8OutputSpecification.XLayer, prediction];
        var y = tensor[Yolo8OutputSpecification.BoxesTensorDimensionLayer, Yolo8OutputSpecification.YLayer, prediction];
        var width = tensor[Yolo8OutputSpecification.BoxesTensorDimensionLayer, Yolo8OutputSpecification.WidthLayer, prediction];
        var height = tensor[Yolo8OutputSpecification.BoxesTensorDimensionLayer, Yolo8OutputSpecification.HeightLayer, prediction];

        return new Rectangle((int)x, (int)y, (int)width, (int)height);
    }
}
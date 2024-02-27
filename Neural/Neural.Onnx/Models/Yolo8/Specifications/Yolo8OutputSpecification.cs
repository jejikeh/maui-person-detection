using System.Collections.Concurrent;
using Microsoft.ML.OnnxRuntime.Tensors;
using Neural.Onnx.Common;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Advanced;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;

namespace Neural.Onnx.Models.Yolo8.Specifications;

public static class Yolo8OutputSpecification
{
    public const int BoxesTensorDimensionLayer = 0;
    public const int SegmentationLayer = 1;
    public const int Boxes = 2;
    public const int BoxesWidthLayer = 3;

    public const int ConfidenceOffsetFromClass = 4;

    public const float ConfidenceThreshold = 0.5f;
    public const float OverlapThreshold = 0.1f;

    public const float ModelQuality = 2f;

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
    
    public static float GetConfidence(this Tensor<float> outputTensor, int classIndex, int box)
    {
        return outputTensor[BoxesTensorDimensionLayer, classIndex + ConfidenceOffsetFromClass, box];
    }
    
    public static bool IsConfidenceThresholdExceeded(this Tensor<float> outputTensor, int classIndex, int boxIndex)
    {
        return outputTensor.GetConfidence(classIndex, boxIndex) <= ConfidenceThreshold;
    }

    private static int GetSegmentationChannelCount(this Tensor<float> outputTensor)
    {
        return outputTensor.Dimensions[SegmentationLayer] - ConfidenceOffsetFromClass - Yolo8Specification.Classes.Length;
    }
    
    public static IEnumerable<SegmentationBoundBox> ToSegmentationBoundBoxes(
        this Tensor<float> segmentationTensor,
        Tensor<float> boxTensor,
        List<IndexedBoundingBox> boxes)
    {
        var segmentationBoundBox = new SegmentationBoundBox[boxes.Count];
    
        var segmentationChannelCount = boxTensor.GetSegmentationChannelCount();

        for (var boxIndex = 0; boxIndex < boxes.Count; boxIndex++)
        {
            var box = boxes[boxIndex];
            
            var maskWeights = boxTensor.ExtractMaskWeights(
                box.Index,
                segmentationChannelCount,
                Yolo8Specification.Classes.Length + ConfidenceOffsetFromClass);

            var mask = segmentationTensor.ExtractMask(
                maskWeights,
                box.Bounds);

            segmentationBoundBox[boxIndex] = new SegmentationBoundBox
            {
                Mask = mask,
                Class = box.Class,
                Bounds = box.Bounds,
                Confidence = box.Confidence
            };
        }

        return segmentationBoundBox;
    }
    
    public static float[] ExtractMaskWeights(this Tensor<float> output, int boxIndex, int maskChannelCount, int maskWeightOffset)
    {
        var maskWeights = new float[maskChannelCount];
    
        for (var maskChannel = 0; maskChannel < maskChannelCount; maskChannel++)
        {
            maskWeights[maskChannel] = output[BoxesTensorDimensionLayer, maskWeightOffset + maskChannel, boxIndex];
        }
        
        return maskWeights;
    }
    
    private static SegmentationMask ExtractMask(
        this Tensor<float> output, 
        IReadOnlyList<float> maskWeights,
        Rectangle maskBound)
    {
        var maskChannels = output.Dimensions[SegmentationLayer] / ModelQuality;
        var maskHeight = output.Dimensions[Boxes];
        var maskWidth = output.Dimensions[BoxesWidthLayer];
        
        using var bitmap = new Image<L8>(maskWidth, maskHeight);

        var pixel = new L8(0);
        
        for (var y = 0; y < maskHeight; y++)
        {
            for (var x = 0; x < maskWidth; x++)
            {
                var value = 0f;

                for (var i = 0; i < maskChannels; i++)
                {
                    value += output[0, i, y, x] * maskWeights[i];
                }

                value = Sigmoid(value);
                pixel.PackedValue = GetLuminance(value);

                bitmap[x, y] = pixel;
            }
        }

        var paddingCropRectangle = new Rectangle(
            0,
            0,
            maskWidth,
            maskHeight);

        bitmap.Mutate(x =>
        {
            x.Crop(paddingCropRectangle);
            x.Resize(Yolo8Specification.InputSize);
            x.Crop(maskBound);
        });

        var final = new float[maskBound.Width, maskBound.Height];

        EnumeratePixels(bitmap, (point, l8) =>
        {
            var confidence = GetConfidence(l8.PackedValue);
            final[point.X, point.Y] = confidence;
        });

        return new SegmentationMask
        {
            Mask = final
        };
    }
    
    private static float Sigmoid(float value)
    {
        var k = MathF.Exp(value);

        return k / (1.0f + k);
    }

    private static byte GetLuminance(float confidence)
    {
        return (byte)((confidence * 255 - 255) * -1);
    }

    private static float GetConfidence(byte luminance)
    {
        return (luminance - 255) * -1 / 255f;
    }

    private static void EnumeratePixels<TPixel>(Image<TPixel> image, Action<Point, TPixel> iterator) where TPixel : unmanaged, IPixel<TPixel>
    {
        var width = image.Width;
        var height = image.Height;

        if (image.DangerousTryGetSinglePixelMemory(out var memory))
        {
            Parallel.For(0, width * height, index =>
            {
                var x = index % width;
                var y = index / width;

                var point = new Point(x, y);
                var pixel = memory.Span[index];

                iterator(point, pixel);
            });
        }
        else
        {
            Parallel.For(0, image.Height, y =>
            {
                var row = image.DangerousGetPixelRowMemory(y).Span;

                for (var x = 0; x < image.Width; x++)
                {
                    var point = new Point(x, y);
                    var pixel = row[x];

                    iterator(point, pixel);
                }
            });
        }
    }
}
using Microsoft.ML.OnnxRuntime.Tensors;
using Neural.Onnx.Common;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;

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
    public const float LuminancePixelThreshold = 0.8f;

    public const float ModelQuality = 2f;

    public const int XLayer = 0;
    public const int YLayer = 1;
    public const int WidthLayer = 2;
    public const int HeightLayer = 3;
    
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

    private static float[] ExtractMaskWeights(this Tensor<float> output, int boxIndex, int maskChannelCount, int maskWeightOffset)
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
        var bitmap = output.FillBitmapImage(maskWeights);

        var maskHeight = output.Dimensions[Boxes];
        var maskWidth = output.Dimensions[BoxesWidthLayer];
        
        bitmap.CropImageInsideMaskBound(maskBound, maskWidth, maskHeight);

        return CalculateMaskConfidence(maskBound, bitmap);
    }

    private static SegmentationMask CalculateMaskConfidence(Rectangle maskBound, Image<L8> bitmap)
    {
        var final = new float[maskBound.Width, maskBound.Height];

        bitmap.EnumeratePixels((point, l8) =>
        {
            var confidence = GetConfidence(l8.PackedValue);
            
            final[point.X, point.Y] = confidence;
        });

        return new SegmentationMask
        {
            Mask = final
        };
    }

    private static Image<L8> FillBitmapImage(this Tensor<float> output, IReadOnlyList<float> maskWeights)
    {
        var maskChannels = output.Dimensions[SegmentationLayer] / ModelQuality;
        
        var maskHeight = output.Dimensions[Boxes];
        var maskWidth = output.Dimensions[BoxesWidthLayer];
        
        var bitmap = new Image<L8>(maskWidth, maskHeight);

        var pixel = new L8(0);
        
        for (var y = 0; y < maskHeight; y++)
        {
            for (var x = 0; x < maskWidth; x++)
            {
                var luminance = output.CalculateTensorLuminance(maskWeights, maskChannels, y, x);
                pixel.PackedValue = LuminanceToByte(luminance);

                bitmap[x, y] = pixel;
            }
        }

        return bitmap;
    }

    private static float CalculateTensorLuminance(
        this Tensor<float> output, 
        IReadOnlyList<float> maskWeights, 
        float maskChannels, 
        int y, 
        int x)
    {
        var value = 0f;

        for (var channel = 0; channel < maskChannels; channel++)
        {
            value += output[BoxesTensorDimensionLayer, channel, y, x] * maskWeights[channel];
        }

        value = Sigmoid(value);

        return value;
    }

    private static float Sigmoid(float value)
    {
        var k = MathF.Exp(value);

        return k / (1.0f + k);
    }

    private static byte LuminanceToByte(float luminance)
    {
        return (byte)((luminance * 255 - 255) * -1);
    }

    private static float GetConfidence(byte confidence)
    {
        return (confidence - 255) * -1 / 255f;
    }
}
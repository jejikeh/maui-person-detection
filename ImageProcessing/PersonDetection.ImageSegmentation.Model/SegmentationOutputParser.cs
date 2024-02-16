using Microsoft.ML.OnnxRuntime.Tensors;
using PersonDetection.ImageSegmentation.Model.Data;
using PersonDetection.ImageSegmentation.Model.Data.Output;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Advanced;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;
using Rectangle = SixLabors.ImageSharp.Rectangle;

namespace PersonDetection.ImageSegmentation.Model;

internal readonly struct SegmentationOutputParser
{
    private static readonly int _classLengthOffset = 4;
    private static readonly int _ignoredLayer = 0;

    public static SegmentationBoundingBox[] Parse(Tensor<float> boxesOutput, Tensor<float> maskPrototypes, Size originSize, bool quantize)
    {
        var reductionRatio = CalculateReductionRatio(originSize);
        var padding = new Padding(originSize, reductionRatio);
        
        var classTensor = boxesOutput.Dimensions[1];
        var maskChannelCount = classTensor - _classLengthOffset - YoloSegmentationOptions.Classes.Length;
        var boxes = IndexedBoundingBoxParser.Parse(boxesOutput, originSize, padding);

        return FillMaskResult(boxesOutput, maskPrototypes, originSize, quantize, boxes, maskChannelCount, padding);
    }

    private static SegmentationBoundingBox[] FillMaskResult(
        Tensor<float> boxesOutput, 
        Tensor<float> maskPrototypes,
        Size originSize, 
        bool quantize,
        IReadOnlyList<IndexedBoundingBox> boxes, 
        int maskChannelCount, 
        Padding padding)
    {
        var result = new SegmentationBoundingBox[boxes.Count];

        for (var index = 0; index < boxes.Count; index++)
        {
            var box = boxes[index];

            var maskWeights = ExtractMaskWeights(boxesOutput, box.Index, maskChannelCount, YoloSegmentationOptions.Classes.Length + 4);
            var mask = ProcessMask(
                maskPrototypes, 
                maskWeights, 
                box.Bounds, 
                originSize, 
                YoloSegmentationOptions.ImageSize, 
                padding, 
                quantize);

            result[index] = new SegmentationBoundingBox
            {
                Mask = mask,
                Class = box.Class,
                Bounds = box.Bounds,
                Confidence = box.Confidence,
            };
        }

        return result;
    }

    private static float CalculateReductionRatio(Size originSize)
    {
        return Math.Min(
            YoloSegmentationOptions.Width / (float)originSize.Width, 
            YoloSegmentationOptions.Height / (float)originSize.Height);
    }

    private static SegmentationMask ProcessMask(
        Tensor<float> maskPrototypes,
        IReadOnlyList<float> maskWeights,
        Rectangle bounds,
        Size originSize,
        Size modelSize,
        Padding padding,
        bool quantize)
    {
        var dimensions = CalculateMaskDimensions(maskPrototypes, quantize);
        using var bitmap = FillMaskBitmap(maskPrototypes, maskWeights, dimensions);

        var xPad = padding.X * dimensions.Width / modelSize.Width;
        var yPad = padding.Y * dimensions.Height / modelSize.Height;

        var paddingCropRectangle = new Rectangle(
            xPad,
            yPad,
            dimensions.Width - xPad * 2, 
            dimensions.Height - yPad * 2);

        bitmap.Mutate(x =>
        {
            x.Crop(paddingCropRectangle);
            x.Resize(originSize);
            x.Crop(bounds);
        });

        var final = new float[bounds.Width, bounds.Height];

        EnumeratePixels(bitmap, (point, pixel) =>
        {
            var confidence = GetConfidence(pixel.PackedValue);
            final[point.X, point.Y] = confidence;
        });

        return new SegmentationMask
        {
            Mask = final
        };
    }

    private static Image<L8> FillMaskBitmap(
        Tensor<float> maskPrototypes, 
        IReadOnlyList<float> maskWeights, 
        MaskDimensions dimensions)
    {
        var bitmap = new Image<L8>(dimensions.Width, dimensions.Height);
        var pixel = new L8(0);
        
        for (var y = 0; y < dimensions.Height; y++)
        {
            for (var x = 0; x < dimensions.Width; x++)
            {
                var value = 0f;
                
                for (var channel = 0; channel < dimensions.Channels; channel++)
                {
                    value += maskPrototypes[_ignoredLayer, channel, y, x] * maskWeights[channel];
                }

                value = Sigmoid(value);
                pixel.PackedValue = GetLuminance(value);

                bitmap[x, y] = pixel;
            }
        }

        return bitmap;
    }

    private static MaskDimensions CalculateMaskDimensions(Tensor<float> maskPrototypes, bool quantize)
    {
        var modelRatio = quantize ? 2 : 1;

        return new MaskDimensions
        {
            Width = maskPrototypes.Dimensions[2],
            Height = maskPrototypes.Dimensions[3],
            Channels = maskPrototypes.Dimensions[1] / modelRatio,
        };
    }

    private static float[] ExtractMaskWeights(Tensor<float> output, int boxIndex, int maskChannelCount, int maskWeightsOffset)
    {
        var maskWeights = new float[maskChannelCount];

        for (var i = 0; i < maskChannelCount; i++)
        {
            maskWeights[i] = output[0, maskWeightsOffset + i, boxIndex];
        }

        return maskWeights;
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
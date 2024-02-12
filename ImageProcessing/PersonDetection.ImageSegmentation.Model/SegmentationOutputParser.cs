using Microsoft.ML.OnnxRuntime.Tensors;
using PersonDetection.ImageSegmentation.Model.Data.Output;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Advanced;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;

namespace PersonDetection.ImageSegmentation.Model;

internal readonly struct SegmentationOutputParser
{
    public static SegmentationBoundingBox[] Parse(Tensor<float> boxesOutput, Tensor<float> maskPrototypes, Size originSize)
    {
        var reductionRatio = Math.Min(YoloSegmentationOptions.Width / (float)originSize.Width, YoloSegmentationOptions.Height / (float)originSize.Height);
        var xPadding = (int)((YoloSegmentationOptions.Width - originSize.Width * reductionRatio) / 2);
        var yPadding = (int)((YoloSegmentationOptions.Height - originSize.Height * reductionRatio) / 2);

        var maskChannelCount = boxesOutput.Dimensions[1] - 4 - YoloSegmentationOptions.Classes.Length;

        var boxes = new IndexedBoundingBoxParser().Parse(boxesOutput, originSize, xPadding, yPadding);

        var result = new SegmentationBoundingBox[boxes.Length];

        for (int index = 0; index < boxes.Length; index++)
        {
            var box = boxes[index];

            var maskWeights = ExtractMaskWeights(boxesOutput, box.Index, maskChannelCount, YoloSegmentationOptions.Classes.Length + 4);

            var mask = ProcessMask(maskPrototypes, maskWeights, box.Bounds, originSize, YoloSegmentationOptions.ImageSize, xPadding, yPadding);

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

    private static SegmentationMask ProcessMask(Tensor<float> maskPrototypes,
        float[] maskWeights,
        Rectangle bounds,
        Size originSize,
        Size modelSize,
        int xPadding,
        int yPadding)
    {
        var maskChannels = maskPrototypes.Dimensions[1];
        var maskHeight = maskPrototypes.Dimensions[2];
        var maskWidth = maskPrototypes.Dimensions[3];

        if (maskChannels != maskWeights.Length)
            throw new InvalidOperationException();

        using var bitmap = new Image<L8>(maskWidth, maskHeight);

        for (var y = 0; y < maskHeight; y++)
        {
            for (var x = 0; x < maskWidth; x++)
            {
                var value = 0F;

                for (var i = 0; i < maskChannels; i++)
                {
                    value += maskPrototypes[0, i, y, x] * maskWeights[i];
                }

                value = Sigmoid(value);

                var color = GetLuminance(value);
                var pixel = new L8(color);

                bitmap[x, y] = pixel;
            }
        }

        var xPad = xPadding * maskWidth / modelSize.Width;
        var yPad = yPadding * maskHeight / modelSize.Height;

        var paddingCropRectangle = new Rectangle(
            xPad,
            yPad,
            maskWidth - xPad * 2, 
            maskHeight - yPad * 2);

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
        return (luminance - 255) * -1 / 255F;
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
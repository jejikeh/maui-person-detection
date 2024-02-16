using Microsoft.ML.OnnxRuntime.Tensors;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Advanced;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;

namespace PersonDetection.ImageSegmentation.Model.Data.Input;

public static class InputImage
{
    private static readonly DenseTensor<float> _input = new DenseTensor<float>(YoloSegmentationOptions.Dimensions);

    public static Tensor<float> ToTensor(this Image<Rgb24> image)
    {
        ProcessToTensor(image, _input);
        
        return _input;
    }
    
    private static void ProcessToTensor(Image<Rgb24> input, DenseTensor<float> target)
    {
        var xPadding = (YoloSegmentationOptions.ImageSize.Width - input.Width) / 2;
        var yPadding = (YoloSegmentationOptions.ImageSize.Height - input.Height) / 2;

        var width = input.Width;
        var height = input.Height;

        var strideBatchR = target.Strides[1] * 0;
        var strideBatchG = target.Strides[1] * 1;
        var strideBatchB = target.Strides[1] * 2;
        var strideY = target.Strides[2];
        var strideX = target.Strides[3];

        var tensorSpan = target.Buffer;

        if (input.DangerousTryGetSinglePixelMemory(out var memory))
        {
            Parallel.For(0, width * height, index =>
            {
                var x = index % width;
                var y = index / width;
                var tensorIndex = strideBatchR + strideY * (y + yPadding) + strideX * (x + xPadding);

                var pixel = memory.Span[index];
                WritePixel(tensorSpan.Span, tensorIndex, pixel, strideBatchR, strideBatchG, strideBatchB);
            });
        }
        else
        {
            Parallel.For(0, height, y =>
            {
                var rowSpan = input.DangerousGetPixelRowMemory(y).Span;
                var tensorYIndex = strideBatchR + strideY * (y + yPadding);

                for (var x = 0; x < width; x++)
                {
                    var tensorIndex = tensorYIndex + strideX * (x + xPadding);
                    var pixel = rowSpan[x];
                    WritePixel(tensorSpan.Span, tensorIndex, pixel, strideBatchR, strideBatchG, strideBatchB);
                }
            });
        }
    }

    private static void WritePixel(Span<float> tensorSpan, int tensorIndex, Rgb24 pixel, int strideBatchR, int strideBatchG, int strideBatchB)
    {
        tensorSpan[tensorIndex] = pixel.R / 255f;
        tensorSpan[tensorIndex + strideBatchG - strideBatchR] = pixel.G / 255f;
        tensorSpan[tensorIndex + strideBatchB - strideBatchR] = pixel.B / 255f;
    }
}
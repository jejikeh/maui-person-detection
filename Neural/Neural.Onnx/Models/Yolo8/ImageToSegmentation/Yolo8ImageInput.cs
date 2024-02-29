using Microsoft.ML.OnnxRuntime;
using Microsoft.ML.OnnxRuntime.Tensors;
using Neural.Core.Models;
using Neural.Onnx.Common;
using Neural.Onnx.Models.Yolo8.Specifications;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Advanced;
using SixLabors.ImageSharp.PixelFormats;

namespace Neural.Onnx.Models.Yolo8.ImageToSegmentation;

public class Yolo8ImageInput(Image<Rgba32> _image) : IModelInput
{
    public readonly Image<Rgba32> Image = _image.ResizeImage(Yolo8Specification.InputSize);

    public NamedOnnxValue[] GetNamedOnnxValues()
    {
        return MapNamedOnnxValues([ToTensor()]);
    }
    
    private NamedOnnxValue[] MapNamedOnnxValues(ReadOnlySpan<Tensor<float>> inputs)
    {
        var inputsLength = inputs.Length;
        var values = new NamedOnnxValue[inputsLength];

        for (var i = 0; i < inputsLength; i++)
        {
            var name = Yolo8Specification.OnnxOutputNames[i];
            values[i] = NamedOnnxValue.CreateFromTensor(name, inputs[i]);
        }

        return values;
    }

    private Tensor<float> ToTensor()
    {
        // var tensor = Yolo8TensorSpecification.Tensor();
        //
        // Parallel.For(0, Yolo8Specification.InputSize.Height, y =>
        // {
        //     Parallel.For(0, Yolo8Specification.InputSize.Width, x =>
        //     {
        //         // We can use Tensor structure from yolo5 model
        //         tensor.FillTensorFromRgbImage(Image, x, y);
        //     });
        // });
        //
        // return tensor;

        var tensor = Yolo8TensorSpecification.Tensor();
        
        ProcessToTensor(Image, tensor);

        return tensor;
    }
    
    private static void ProcessToTensor(Image<Rgba32> input, DenseTensor<float> target)
    {
        var xPadding = (Yolo8Specification.InputSize.Width - input.Width) / 2;
        var yPadding = (Yolo8Specification.InputSize.Height - input.Height) / 2;

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

    private static void WritePixel(Span<float> tensorSpan, int tensorIndex, Rgba32 pixel, int strideBatchR, int strideBatchG, int strideBatchB)
    {
        tensorSpan[tensorIndex] = pixel.R / 255f;
        tensorSpan[tensorIndex + strideBatchG - strideBatchR] = pixel.G / 255f;
        tensorSpan[tensorIndex + strideBatchB - strideBatchR] = pixel.B / 255f;
    }
}
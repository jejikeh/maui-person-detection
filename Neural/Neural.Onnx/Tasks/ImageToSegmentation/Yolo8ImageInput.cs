using Microsoft.ML.OnnxRuntime;
using Microsoft.ML.OnnxRuntime.Tensors;
using Neural.Core.Models;
using Neural.Onnx.Common;
using Neural.Onnx.Models.Yolo5.Specifications;
using Neural.Onnx.Models.Yolo8.Specifications;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;

namespace Neural.Onnx.Tasks.ImageToSegmentation;

public class Yolo8ImageInput(Image<Rgba32> _image, string[] _labels) : IModelInput
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
            var name = _labels[i];
            var value = NamedOnnxValue.CreateFromTensor(name, inputs[i]);
            values[i] = value;
        }

        return values;
    }

    private Tensor<float> ToTensor()
    {
        var tensor = Yolo8TensorSpecification.Tensor();
        
        Parallel.For(0, Yolo8Specification.InputSize.Height, y =>
        {
            Parallel.For(0, Yolo8Specification.InputSize.Width, x =>
            {
                // We can use Tensor structure from yolo5 model
                tensor.FillTensorFromRgbImage(Image, x, y);
            });
        });
        
        return tensor;
    }
}
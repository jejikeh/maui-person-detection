using Microsoft.ML.OnnxRuntime;
using Microsoft.ML.OnnxRuntime.Tensors;
using Neural.Core.Models;
using Neural.Onnx.Models.Yolo5;
using Neural.Onnx.Models.Yolo5.Specifications;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;

namespace Neural.Onnx.Tasks.ImageToBoxPredictions;

public class Yolo5ImageInput(Image<Rgba32> _image) : IModelInput
{
    private readonly Image<Rgba32> _image = ResizeImage(_image.Clone(), Yolo5Specification.InputSize);
    
    public List<NamedOnnxValue> GetNamedOnnxValues()
    {
        return [NamedOnnxValue.CreateFromTensor(Yolo5InputSpecification.TensorName, ToTensor())];
    }

    private Tensor<float> ToTensor()
    {
        var tensor = Yolo5TensorSpecification.Tensor();
        
        Parallel.For(0, Yolo5Specification.InputSize.Height, y =>
        {
            Parallel.For(0, Yolo5Specification.InputSize.Width, x =>
            {
                tensor.FillTensorRgb(_image, x, y);
            });
        });
        
        return tensor;
    }

    private static Image<Rgba32> ResizeImage(Image<Rgba32> image, Size size)
    {
        if (image.Width != size.Width || image.Height != size.Height)
        {
            image.Mutate(x => x.Resize(size.Width, size.Height));
        }

        return image;
    }
}
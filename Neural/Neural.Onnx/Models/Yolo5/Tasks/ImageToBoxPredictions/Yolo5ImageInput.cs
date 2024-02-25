using Microsoft.ML.OnnxRuntime;
using Microsoft.ML.OnnxRuntime.Tensors;
using Neural.Core.Models;
using Neural.Onnx.Common;
using Neural.Onnx.Models.Yolo5.Specifications;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;

namespace Neural.Onnx.Models.Yolo5.Tasks.ImageToBoxPredictions;

public class Yolo5ImageInput(Image<Rgba32> _image) : IModelInput
{
    public readonly Image<Rgba32> Image = _image.ResizeImage(Yolo5Specification.InputSize);
    
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
                tensor.FillTensorFromRgbImage(Image, x, y);
            });
        });
        
        return tensor;
    }
}
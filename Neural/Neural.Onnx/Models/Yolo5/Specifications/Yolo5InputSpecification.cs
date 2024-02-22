using Microsoft.ML.OnnxRuntime.Tensors;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;

namespace Neural.Onnx.Models.Yolo5.Specifications;

public static class Yolo5InputSpecification
{
    public const int BatchLayer = 0;
    public const int RedLayer = 0;
    public const int GreenLayer = 1;
    public const int BlueLayer = 2;
    
    public static string TensorName { get; set; } = "images";
    
    private const float _maxColorValue = 255.0f; 
    
    public static DenseTensor<float> FillTensorRgb(this DenseTensor<float> tensor, Image<Rgba32> image, int x, int y)
    {
        return tensor
            .FillTensor(image, x, y, RedLayer)
            .FillTensor(image, x, y, GreenLayer)
            .FillTensor(image, x, y, BlueLayer);
    }

    public static DenseTensor<float> FillTensor(this DenseTensor<float> tensor, Image<Rgba32> image, int x, int y, int layer)
    {
        var pixelColorValue = layer switch
        {
            RedLayer => image[x, y].R,
            GreenLayer => image[x, y].G,
            BlueLayer => image[x, y].B,
            _ => throw new ArgumentOutOfRangeException(nameof(layer), layer, null)
        } / _maxColorValue;

        tensor[BatchLayer, layer, y, x] = pixelColorValue;
        
        return tensor;
    }
}
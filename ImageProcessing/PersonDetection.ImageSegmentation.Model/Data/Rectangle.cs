using Microsoft.ML.OnnxRuntime.Tensors;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;

namespace PersonDetection.ImageSegmentation.Model.Data;

public class Rectangle
{
    public float X { get; set; }
    public float Y { get; set; }
    public float Width { get; set; }
    public float Height { get; set; }

    public static Rectangle FromTensor(Tensor<float> output, int layer)
    {
        var x = output[0, 0, layer];
        var y = output[0, 1, layer];
        var w = output[0, 2, layer];
        var h = output[0, 3, layer];

        return new Rectangle
        {
            X = x,
            Y = y,
            Width = w,
            Height = h
        };
    }
}
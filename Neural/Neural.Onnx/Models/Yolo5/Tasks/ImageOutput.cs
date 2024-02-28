using Neural.Core.Models;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;

namespace Neural.Onnx.Models.Yolo5.Tasks;

public class ImageOutput : IModelOutput
{
    public Image<Rgba32>? Image { get; private set; }
    
    public void Set(object value)
    {
        if (value is not Image<Rgba32> image)
        {
            throw new ArgumentException(nameof(value));
        }
        
        Image = image;
    }
}
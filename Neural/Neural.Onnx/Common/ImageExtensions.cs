using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;

namespace Neural.Onnx.Common;

public static class ImageExtensions
{
    public static Image<Rgba32> ResizeImage(this Image<Rgba32> image, Size size)
    {
        if (image.Width != size.Width || image.Height != size.Height)
        {
            image.Mutate(x => x.Resize(size.Width, size.Height));
        }

        return image;
    }
}
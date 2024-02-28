using Neural.Onnx.Models.Yolo8.Specifications;
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
    
    public static void PutPixelFromMask(
        this Image<Rgba32> masksLayer, 
        SegmentationBoundBox box, 
        int x, 
        int maskStartX, 
        int y,
        int maskStartY)
    {
        var value = box.Mask[x - maskStartX, y - maskStartY];

        if (value > 0.74f)
        {
            masksLayer[x, y] = Color.LightGreen;
        }
    }
    
    public static void DrawSegmentationBox(this Image<Rgba32> masksLayer, SegmentationBoundBox box)
    {
        var maskBounds = box.Bounds;

        var maskStartX = maskBounds.X;
        var maskEndX = maskStartX + maskBounds.Width;
        var maskStartY = maskBounds.Y;
        var maskEndY = maskStartY + maskBounds.Height;

        for (var x = maskStartX; x < maskEndX; x++)
        {
            for (var y = maskStartY; y < maskEndY; y++)
            {
                masksLayer.PutPixelFromMask(box, x, maskStartX, y, maskStartY);
            }
        }
    }
}
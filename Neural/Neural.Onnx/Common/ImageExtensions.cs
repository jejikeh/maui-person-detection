using Neural.Onnx.Models.Yolo8.Specifications;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Advanced;
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
    
    public static void EnumeratePixels<TPixel>(this Image<TPixel> image, Action<Point, TPixel> iterator) where TPixel : unmanaged, IPixel<TPixel>
    {
        var width = image.Width;
        var height = image.Height;

        if (image.DangerousTryGetSinglePixelMemory(out var memory))
        {
            Parallel.For(0, width * height, index =>
            {
                var x = index % width;
                var y = index / width;

                var point = new Point(x, y);
                var pixel = memory.Span[index];

                iterator(point, pixel);
            });
        }
        else
        {
            Parallel.For(0, image.Height, y =>
            {
                var row = image.DangerousGetPixelRowMemory(y).Span;

                for (var x = 0; x < image.Width; x++)
                {
                    var point = new Point(x, y);
                    var pixel = row[x];

                    iterator(point, pixel);
                }
            });
        }
    }
    
    public static void CropImageInsideMaskBound(this Image<L8> bitmap, Rectangle maskBound, int maskWidth, int maskHeight)
    {
        var paddingCropRectangle = new Rectangle(
            0,
            0,
            maskWidth,
            maskHeight);

        bitmap.Mutate(x =>
        {
            x.Crop(paddingCropRectangle);
            x.Resize(Yolo8Specification.InputSize);
            x.Crop(maskBound);
        });
    }

    private static void PutPixelFromMask(
        this Image<Rgba32> masksLayer, 
        SegmentationBoundBox box, 
        int x, 
        int maskStartX, 
        int y,
        int maskStartY)
    {
        var value = box.Mask[x - maskStartX, y - maskStartY];

        if (value > Yolo8OutputSpecification.LuminancePixelThreshold)
        {
            masksLayer[x, y] = Color.LightGreen;
        }
    }
}
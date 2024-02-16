using PersonDetection.ImageSegmentation.Model.Data;
using PersonDetection.ImageSegmentation.Model.Data.Output;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;
using Rectangle = SixLabors.ImageSharp.Rectangle;

namespace PersonDetection.ImageSegmentation.Model;

public static class PlottingExtensions
{
    public static Image DrawSegmentations(this Segmentation result, Image originImage)
    {
        var size = originImage.Size;
        var masksImageLayer = CreateTransparentImage(size);
        
        foreach (var box in result.Boxes)
        {
            if (!box.Class.Equals(YoloSegmentationOptions.PersonClass))
            {
                continue;
            }

            masksImageLayer = FillMaskImageDetectionLayer(box);
        }

        originImage.Mutate(x => x.DrawImage(masksImageLayer, 1f));
        
        return originImage;
    }

    private static Image<Rgba32> FillMaskImageDetectionLayer(SegmentationBoundingBox box)
    {
        var detectionMask = CreateTransparentImage(box.Bounds);

        for (var x = 0; x < box.Mask.Width; x++)
        {
            for (var y = 0; y < box.Mask.Height; y++)
            {
                var value = box.Mask[x, y];

                if (value > 0.74f)
                {
                    detectionMask[x, y] = Color.LightGreen;
                }
            }
        }
        
        detectionMask.Mutate(x => x.DrawImage(detectionMask, box.Bounds.Location, 1f));

        return detectionMask;
    }

    private static Image<Rgba32> CreateTransparentImage(Size size)
    {
        return new Image<Rgba32>(size.Width, size.Height, ColorPalette.Transparent);
    }

    private static Image<Rgba32> CreateTransparentImage(Rectangle rectangle)
    {
        return new Image<Rgba32>(rectangle.Width, rectangle.Height, ColorPalette.Transparent);
    }
}
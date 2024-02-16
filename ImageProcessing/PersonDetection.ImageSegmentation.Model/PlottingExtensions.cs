using PersonDetection.ImageSegmentation.Model.Data.Input;
using PersonDetection.ImageSegmentation.Model.Data.Output;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;

namespace PersonDetection.ImageSegmentation.Model;

public static class PlottingExtensions
{
    public static Image PlotImage(this Segmentation result, Image originImage)
    {
        var size = originImage.Size;
        using var masksLayer = new Image<Rgba32>(size.Width, size.Height, new Rgba32(0,0,0,0));
        
        foreach (var box in result.Boxes)
        {
            if (box.Class != "person")
            {
                continue;
            }
            
            using var mask = new Image<Rgba32>(box.Bounds.Width, box.Bounds.Height, new Rgba32(0,0,0,0));

            for (var x = 0; x < box.Mask.Width; x++)
            {
                for (var y = 0; y < box.Mask.Height; y++)
                {
                    var value = box.Mask[x, y];

                    if (value > 0.74f)
                    {
                        mask[x, y] = Color.LightGreen;
                    }
                }
            }

            masksLayer.Mutate(x => x.DrawImage(mask, box.Bounds.Location, 1f));
        }

        originImage.Mutate(x => x.DrawImage(masksLayer, 1f));
        
        return originImage;
    }
}
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
        using var masksLayer = new Image<Rgba32>(size.Width, size.Height);
        
        foreach (var box in result.Boxes)
        {
            using var mask = new Image<Rgba32>(box.Bounds.Width, box.Bounds.Height);

            for (var x = 0; x < box.Mask.Width; x++)
            {
                for (var y = 0; y < box.Mask.Height; y++)
                {
                    var value = box.Mask[x, y];

                    if (value > 0.65f)
                    {
                        mask[x, y] = Color.Red;
                    }
                }
            }

            masksLayer.Mutate(x => x.DrawImage(mask, box.Bounds.Location, 1F));
        }

        originImage.Mutate(x => x.DrawImage(masksLayer, 0.4F));
        
        return originImage;
    }
}
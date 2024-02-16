using SixLabors.ImageSharp;

namespace PersonDetection.ImageSegmentation.Model.Data;

public class Padding(Size originSize, float reductionRatio)
{
    public int X = (int)((YoloSegmentationOptions.Width - originSize.Width * reductionRatio) / 2);
    public int Y = (int)((YoloSegmentationOptions.Height - originSize.Height * reductionRatio) / 2);
}
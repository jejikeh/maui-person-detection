using SixLabors.ImageSharp;

namespace PersonDetection.ImageSegmentation.Model.Data.Output;

public class Segmentation
{
    public required SegmentationBoundingBox[] Boxes { get; set; }
}
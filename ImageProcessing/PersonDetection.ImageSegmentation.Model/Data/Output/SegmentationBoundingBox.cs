using SixLabors.ImageSharp;

namespace PersonDetection.ImageSegmentation.Model.Data.Output;

public class SegmentationBoundingBox
{
    public required SegmentationMask Mask { get; init; }
    public required string Class { get; init; }
    public required Rectangle Bounds { get; init; }
    public required float Confidence { get; init; }
}
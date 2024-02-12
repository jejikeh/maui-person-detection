using SixLabors.ImageSharp;

namespace PersonDetection.ImageSegmentation.Model.Data.Output;

public class BoundingBox
{
    public required string Class { get; init; }
    public required Rectangle Bounds { get; init; }
    public required float Confidence { get; init; }
}
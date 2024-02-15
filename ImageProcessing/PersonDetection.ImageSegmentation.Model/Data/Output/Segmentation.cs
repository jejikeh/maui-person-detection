namespace PersonDetection.ImageSegmentation.Model.Data.Output;

public class Segmentation
{
    public required SegmentationBoundingBox[] Boxes { get; init; }
}
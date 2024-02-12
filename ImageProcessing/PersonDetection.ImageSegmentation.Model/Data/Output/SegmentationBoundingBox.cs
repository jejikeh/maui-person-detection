namespace PersonDetection.ImageSegmentation.Model.Data.Output;

public class SegmentationBoundingBox : BoundingBox
{
    public required SegmentationMask Mask { get; init; }
}
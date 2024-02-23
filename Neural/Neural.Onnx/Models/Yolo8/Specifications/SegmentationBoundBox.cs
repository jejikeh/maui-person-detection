using SixLabors.ImageSharp;

namespace Neural.Onnx.Models.Yolo8.Specifications;

public class SegmentationBoundBox
{
    public required SegmentationMask Mask { get; init; }
    public required YoloClass Class { get; init; }
    public required Rectangle Bounds { get; init; }
    public required float Confidence { get; init; }
}
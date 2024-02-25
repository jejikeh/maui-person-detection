using Microsoft.ML.OnnxRuntime.Tensors;
using SixLabors.ImageSharp;

namespace Neural.Onnx.Models.Yolo8.Specifications;

public readonly struct IndexedBoundingBox : IComparable<IndexedBoundingBox>
{
    public bool IsEmpty => Bounds.IsEmpty;
    public required int Index { get; init; }
    public required string Class { get; init; }
    public required Rectangle Bounds { get; init; }
    public required float Confidence { get; init; }
    
    public int CompareTo(IndexedBoundingBox other) => Confidence.CompareTo(other.Confidence);

    // public static SegmentationBoundBox[] ToSegmentationBoundBoxes(this IndexedBoundingBox[] boxes)
    // {
    //     var segmentationBoundBox = new SegmentationBoundBox[];
    //     
    // }
}
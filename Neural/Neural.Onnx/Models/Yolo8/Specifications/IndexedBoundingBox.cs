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

    // public static IndexedBoundingBox[] ExtractIndexedBoundingBoxes(Tensor<float> tensor)
    // {
    //     var boxesCount = tensor.Dimensions[Yolo8OutputSpecification.BoxesLayer];
    //     var boxes = new IndexedBoundingBox[boxesCount];
    //
    //     Parallel.For(0, boxesCount, boxIndex =>
    //     {
    //         for (var classIndex = 0; classIndex < Yolo8Specification.Classes.Length; classIndex++)
    //         {
    //             var confidence = tensor.GetConfidence(boxIndex, classIndex);
    //         }
    //     });
    // }
}
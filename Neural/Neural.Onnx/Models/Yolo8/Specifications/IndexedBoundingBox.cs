using Microsoft.ML.OnnxRuntime.Tensors;
using Neural.Onnx.Common;
using SixLabors.ImageSharp;

namespace Neural.Onnx.Models.Yolo8.Specifications;

public readonly struct IndexedBoundingBox : IComparable<IndexedBoundingBox>
{
    public bool IsEmpty => Bounds.IsEmpty;
    public required int Index { get; init; }
    public required YoloClass Class { get; init; }
    public required Rectangle Bounds { get; init; }
    public required float Confidence { get; init; }
    
    public int CompareTo(IndexedBoundingBox other) => Confidence.CompareTo(other.Confidence);

    public static List<IndexedBoundingBox> FilterOverlappingBoxes(IReadOnlyList<IndexedBoundingBox> boxes)
    {
        var filteredIndexes = new List<int>();
        
        foreach(var (boxIndex, box) in boxes.Select((value, index) => (index, value)))
        {
            if (filteredIndexes.Contains(boxIndex))
            {
                continue;
            }
            
            filteredIndexes.Add(boxIndex);

            filteredIndexes.RemoveAll(otherBoxIndex =>
            {
                var otherBox = boxes[otherBoxIndex];
                    
                return box.Bounds.IsOverlappingAboveThreshold(
                    otherBox.Bounds, 
                    Yolo8OutputSpecification.OverlapThreshold);
            });
        }
        
        return filteredIndexes.Select(boxes.ElementAt).ToList();
    }
}
using Neural.Onnx.Common;
using SixLabors.ImageSharp;

namespace Neural.Onnx.Models.Yolo8.Specifications;

public readonly struct IndexedBoundingBox : IComparable<IndexedBoundingBox>
{
    public required int Index { get; init; }
    public required YoloClass Class { get; init; }
    public required Rectangle Bounds { get; init; }
    public required float Confidence { get; init; }
    
    public int CompareTo(IndexedBoundingBox other) => Confidence.CompareTo(other.Confidence);

    public static List<IndexedBoundingBox> FilterOverlappingBoxes(IReadOnlyList<IndexedBoundingBox> boxes)
    {
        var activeBoxes = new HashSet<int>(Enumerable.Range(0, boxes.Count));

        var selected = new List<IndexedBoundingBox>();
    
        while(activeBoxes.Count != 0)
        {
            var currentBoxIndex = activeBoxes.First();
        
            activeBoxes.Remove(currentBoxIndex);
        
            var currentBox = boxes[currentBoxIndex];
            selected.Add(currentBox);

            foreach (var otherBoxIndex in activeBoxes)
            {
                var otherBox = boxes[otherBoxIndex];

                if (currentBox.Bounds.IsOverlappingAboveThreshold(otherBox.Bounds, Yolo8OutputSpecification.OverlapThreshold))
                {
                    activeBoxes.Remove(otherBoxIndex);
                }
            }
        }
    
        return selected;
    }
}
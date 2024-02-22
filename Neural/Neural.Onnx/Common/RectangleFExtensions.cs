using SixLabors.ImageSharp;

namespace Neural.Onnx.Common;

public static class RectangleFExtensions
{
    public static float Area(this RectangleF source)
    {
        return source.Width * source.Height;
    }

    public static float CalculateOverlapArea(this RectangleF intersection, RectangleF rectangle1, RectangleF rectangle2)
    {
        var intersectionArea = intersection.Area();
        
        var unionArea = rectangle1.Area() + rectangle2.Area() - intersectionArea;
        
        return intersectionArea / unionArea;
    }
    
    public static bool IsOverlappingAboveThreshold(this RectangleF intersection, RectangleF rectangle1, RectangleF rectangle2, float threshold) => 
        intersection.CalculateOverlapArea(rectangle1, rectangle2) > threshold;
}
using SixLabors.ImageSharp;

namespace Neural.Onnx.Common;

public static class RectangleExtensions
{
    public static float Area(this RectangleF source)
    {
        return source.Width * source.Height;
    }

    public static float Area(this Rectangle source)
    {
        return source.Width * source.Height;
    }

    public static float CalculateOverlapArea(this RectangleF intersection, RectangleF rectangle1, RectangleF rectangle2)
    {
        var intersectionArea = intersection.Area();
        
        var unionArea = rectangle1.Area() + rectangle2.Area() - intersectionArea;
        
        return intersectionArea / unionArea;
    }

    public static float CalculateOverlapArea(this Rectangle rectangle1, Rectangle rectangle2)
    {
        var areaA = rectangle1.Area();

        if (areaA <= 0f)
        {
            return 0f;
        }
        
        var areaB = rectangle2.Area();

        if (areaB <= 0f)
        {
            return 0f;
        }
        
        var intersection = Rectangle.Intersect(rectangle1, rectangle2).Area();
        
        return intersection / areaA + areaB - intersection;
    }
    
    public static bool IsOverlappingAboveThreshold(
        this Rectangle rectangle1,
        Rectangle rectangle2, 
        float threshold) => rectangle1.CalculateOverlapArea(rectangle2) > threshold;
    
    public static bool IsOverlappingAboveThreshold(
        this RectangleF intersection, 
        RectangleF rectangle1, 
        RectangleF rectangle2, 
        float threshold) => intersection.CalculateOverlapArea(rectangle1, rectangle2) > threshold;
    
    public static Rectangle CenterRectangle(this Rectangle rectangle)
    {
        var xMin = rectangle.X - rectangle.Width / 2;
        var yMin = rectangle.Y - rectangle.Height / 2;
        var xMax = rectangle.X + rectangle.Width / 2;
        var yMax = rectangle.Y + rectangle.Height / 2;
        
        return Rectangle.FromLTRB(xMin, yMin, xMax, yMax);
    }
}
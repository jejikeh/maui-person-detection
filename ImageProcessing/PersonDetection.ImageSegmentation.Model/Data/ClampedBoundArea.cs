using SixLabors.ImageSharp;

namespace PersonDetection.ImageSegmentation.Model.Data;

public class ClampedBoundArea
{
    public int MaxX { get; set; }
    public int MaxY { get; set; }
    public int MinX { get; set; }
    public int MinY { get; set; }

    public static ClampedBoundArea FromAreaInsideMaxRatio(Rectangle rectangle, float maxRatio, Size originSize, Padding padding)
    {
        var clampedBoundArea = CalculateNonClampedValues(maxRatio, rectangle, padding);
        clampedBoundArea.ClampToOrigin(originSize);

        return clampedBoundArea;
    }

    public SixLabors.ImageSharp.Rectangle ToRectangle()
    {
        return SixLabors.ImageSharp.Rectangle.FromLTRB(MinX, MinY, MaxX, MaxY);
    }

    private void ClampToOrigin(Size origin)
    {
        MinX = Math.Clamp(MinX, 0, origin.Width);
        MinY = Math.Clamp(MinY, 0, origin.Height);
        MaxX = Math.Clamp(MaxX, 0, origin.Width);
        MaxY = Math.Clamp(MaxY, 0, origin.Height);
    }

    private static ClampedBoundArea CalculateNonClampedValues(float maxRatio, Rectangle rectangle, Padding padding)
    {
        return new ClampedBoundArea()
        {
            MinX = (int)((rectangle.X - rectangle.Width / 2 - padding.X) * maxRatio),
            MinY = (int)((rectangle.Y - rectangle.Height / 2 - padding.Y) * maxRatio),
            MaxX = (int)((rectangle.X + rectangle.Width / 2 - padding.X) * maxRatio),
            MaxY = (int)((rectangle.Y + rectangle.Height / 2 - padding.Y) * maxRatio),
        };
    }
}
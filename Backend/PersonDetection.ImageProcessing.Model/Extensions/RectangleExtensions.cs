using SixLabors.ImageSharp;

namespace PersonDetection.ImageProcessing.Model.Extensions;

public static class RectangleExtensions
{
    /// <summary>
    /// Calculates rectangle area.
    /// </summary>
    public static float Area(this RectangleF source)
    {
        return source.Width * source.Height;
    }
}

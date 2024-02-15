using SixLabors.ImageSharp;

namespace PersonDetection.ImageProcessing.Model;

public record YoloLabel(string Name, Color Color)
{
    public YoloLabel(int id, string name) : this(name, Color.Yellow) { }
}

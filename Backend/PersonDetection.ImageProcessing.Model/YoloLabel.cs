using SixLabors.ImageSharp;

namespace PersonDetection.ImageProcessing.Model;

public record YoloLabel(int Id, string Name, Color Color, YoloLabelKind Kind)
{
    public YoloLabel(int id, string name) : this(id, name, Color.Yellow, YoloLabelKind.Generic) { }
}

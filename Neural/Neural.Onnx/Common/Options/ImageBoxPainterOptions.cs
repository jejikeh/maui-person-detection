namespace Neural.Onnx.Common.Options;

public class ImageBoxPainterOptions
{
    public int ScoreDigits { get; set; } =  2;
    public int LeftRectangleOffset { get; set; } = 3;
    public int TopRectangleOffset { get; set; } = 23;

    public int StrokeBoxWidth { get; set; } = 3;
    public int StrokeTextWidth { get; set; } = 1;

    public int FontSize { get; set; } = 16;
}
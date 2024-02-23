using Neural.Onnx.Common;
using Neural.Onnx.Models.Yolo5.Specifications;
using SixLabors.Fonts;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Drawing.Processing;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;

namespace Neural.Onnx.Services.Implementations;

public class ImageBoxPainterService : IImageBoxPainterService
{
    private const int _fontSize = 16;
    private static Font? _font;
    
    public void PaintPredictions(Image<Rgba32> image, List<Yolo5Prediction> predictions)
    {
        if (_font is null)
        {
            InitializeFont();
        }

        foreach (var prediction in predictions)
        {
            var score = Math.Round(prediction.Score, 2);
            var x = prediction.Rectangle.Left - 3; 
            var y = prediction.Rectangle.Top - 23;

            image.Mutate(
                imageProcessingContext => 
                    imageProcessingContext.DrawPolygon(
                        new SolidPen(new SolidBrush(Color.Red), 3), 
                        new PointF(prediction.Rectangle.Left, prediction.Rectangle.Top), 
                        new PointF(prediction.Rectangle.Right, prediction.Rectangle.Top), 
                        new PointF(prediction.Rectangle.Right, prediction.Rectangle.Bottom), 
                        new PointF(prediction.Rectangle.Left, prediction.Rectangle.Bottom)));
            
            image.Mutate(
                imageProcessingContext => 
                    imageProcessingContext.DrawText($"{prediction.Class.DisplayName()} ({score})", 
                        _font!, 
                        new SolidBrush(Color.Red), 
                        new PointF(x, y)));
        }
    }
    
    private static void InitializeFont()
    {
        // I cant just iterate over SystemFonts.Families and pick random one.
        // This collection may contain a system font that doesn't have a latin variant.
        if (!SystemFonts.TryGet("Roboto", out var fontFamily))
        {
            // Arial have almost all operating systems
            fontFamily = SystemFonts.Get("Arial");
        }
        
        _font ??= fontFamily.CreateFont(_fontSize);
    }
}
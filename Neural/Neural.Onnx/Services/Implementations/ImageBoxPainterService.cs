using Neural.Onnx.Common;
using Neural.Onnx.Models;
using Neural.Onnx.Models.Yolo5.Specifications;
using Neural.Onnx.Models.Yolo8.Specifications;
using SixLabors.Fonts;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Drawing.Processing;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;

namespace Neural.Onnx.Services.Implementations;

public class ImageBoxPainterService : IImageBoxPainterService
{
    private const int _scoreDigits = 2;
    private const int _leftRectangleOffset = 3;
    private const int _topRectangleOffset = 23;
    
    private const int _strokeBoxWidth = 3;
    private const int _strokeTextWidth = 1;
    
    private const int _fontSize = 16;
    private static Font? _font;
    
    private static readonly SolidBrush _redBrush = new SolidBrush(Color.Red);
    
    private static readonly SolidPen _redPenForBoxes = new SolidPen(_redBrush, _strokeBoxWidth);
    private static readonly SolidPen _redPenForText = new SolidPen(_redBrush, _strokeTextWidth);

    public void PaintPredictions(Image<Rgba32> image, IEnumerable<Yolo5Prediction> predictions)
    {
        if (_font is null)
        {
            InitializeFont();
        }

        predictions.AsParallel().ForAll(prediction =>
        {
            image.Mutate(imageProcessingContext =>
            {
                DrawBoxes(imageProcessingContext, prediction);
                DrawText(imageProcessingContext, prediction);
            });
        });
    }

    public void PaintPersonPredictions(Image<Rgba32> image, IEnumerable<SegmentationBoundBox> predictions)
    {
        var size = image.Size;
        var masksLayer = new Image<Rgba32>(size.Width, size.Height);

        foreach (var box in predictions.Where(b => b.Class == YoloClass.Person))
        {
            masksLayer.DrawSegmentationBox(box);
        }

        image.Mutate(context => context.DrawImage(masksLayer, 0.7f));
    }

    private static void DrawText(IImageProcessingContext imageProcessingContext, Yolo5Prediction prediction)
    {
        var score = Math.Round(prediction.Score, _scoreDigits);
        
        var textX = prediction.Rectangle.Left - _leftRectangleOffset; 
        var textY = prediction.Rectangle.Top - _topRectangleOffset;
        
        imageProcessingContext.DrawText($"{prediction.Class.DisplayName()} ({score})", 
            _font!, 
            _redPenForText, 
            new PointF(textX, textY));
    }

    private static void DrawBoxes(IImageProcessingContext imageProcessingContext, Yolo5Prediction prediction)
    {
        imageProcessingContext.DrawPolygon(
            _redPenForBoxes,
            new PointF(prediction.Rectangle.Left, prediction.Rectangle.Top),
            new PointF(prediction.Rectangle.Right, prediction.Rectangle.Top),
            new PointF(prediction.Rectangle.Right, prediction.Rectangle.Bottom),
            new PointF(prediction.Rectangle.Left, prediction.Rectangle.Bottom));
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
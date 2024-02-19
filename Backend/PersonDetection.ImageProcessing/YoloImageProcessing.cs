using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using PersonDetection.ImageProcessing.Model;
using PersonDetection.ImageProcessing.Options;
using SixLabors.Fonts;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Drawing.Processing;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;

namespace PersonDetection.ImageProcessing;

// @Note: Original source code - https://github.com/techwingslab/yolov5-net
// This version is a fix of YoloV5-net.
// Some of method were outdated and not cross-platform.
public class YoloImageProcessing(IOptions<YoloImageProcessingOptions> options) : IDisposable
{
    private YoloScorer _scorer;
    private Font _font;
    
    public async Task<string> Predict(string base64Image)
    {
        if (_scorer is null || _font is null)
        {
            await InitializeAsync();
        }
        
        var image = Image.Load<Rgba32>(Convert.FromBase64String(base64Image));
        var predictions = _scorer!.Predict(image);

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
                    imageProcessingContext.DrawText($"{prediction.Label.Name} ({score})", 
                        _font, 
                        prediction.Label.Color, 
                        new PointF(x, y)));
        }

        var stream = new MemoryStream();
        await image.SaveAsPngAsync(stream);
        var base64 = Convert.ToBase64String(stream.ToArray());
        
        return base64;
    }
    
    public void Dispose()
    {
        _scorer?.Dispose();
    }
    
    private async Task InitializeAsync()
    {
        _scorer ??= await YoloScorer.CreateAsync(options.Value.WeightsPath);
        _font ??= SystemFonts.Get("Roboto").CreateFont(options.Value.FontSize);
    }
}
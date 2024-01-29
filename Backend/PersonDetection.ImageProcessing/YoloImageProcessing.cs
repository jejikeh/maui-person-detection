using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Maui.Storage;
using PersonDetection.ImageProcessing.Configuration;
using PersonDetection.ImageProcessing.Model;
using SixLabors.Fonts;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Drawing.Processing;
using SixLabors.ImageSharp.Formats.Png;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;

namespace PersonDetection.ImageProcessing;

public class YoloImageProcessing(IImageProcessingConfiguration configuration) : IDisposable
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
            var (x, y) = (prediction.Rectangle.Left - 3, prediction.Rectangle.Top - 23);

            image.Mutate(
                a => a.DrawPolygon(new SolidPen(new SolidBrush(Color.Red)),
                new PointF(prediction.Rectangle.Left, prediction.Rectangle.Top),
                new PointF(prediction.Rectangle.Right, prediction.Rectangle.Top),
                new PointF(prediction.Rectangle.Right, prediction.Rectangle.Bottom),
                new PointF(prediction.Rectangle.Left, prediction.Rectangle.Bottom)
            ));
            
            image.Mutate(
                a => a.DrawText($"{prediction.Label.Name} ({score})",
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
        _scorer ??= await YoloScorer.CreateAsync(configuration.WeightsPath);
        var fontCollection = SystemFonts.Families.FirstOrDefault();
        _font ??= fontCollection.CreateFont(configuration.FontSize);
    }
}
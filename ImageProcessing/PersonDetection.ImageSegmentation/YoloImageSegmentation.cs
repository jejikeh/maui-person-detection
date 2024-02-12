using PersonDetection.ImageSegmentation.Model;
using PersonDetection.ImageSegmentation.Model.Data.Output;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;

namespace PersonDetection.ImageSegmentation.ModelConverter;

public class YoloImageSegmentation
{
    private readonly Instance _instance = new();
    
    public async Task<string> SegmentAsync(string base64Image)
    {
        var image = Image.Load<Rgb24>(Convert.FromBase64String(base64Image));
        
        var segmentation = _instance.Predict(image);
        var output = segmentation.PlotImage(image);
        
        var stream = new MemoryStream();
        await output.SaveAsPngAsync(stream);
        var base64 = Convert.ToBase64String(stream.ToArray());
        
        return base64;
    }
    
    public Segmentation CalculateSegmentation(string base64Image)
    {
        var image = Image.Load<Rgb24>(Convert.FromBase64String(base64Image));
        
        var segmentation = _instance.Predict(image);

        return segmentation;
    }

    public async Task<string> DrawSegmentationAsync(Segmentation segmentation)
    {
        var image = new Image<Rgb24>(640, 480);
        var output = segmentation.PlotImage(image);
        
        var stream = new MemoryStream();
        await output.SaveAsPngAsync(stream);
        var base64 = Convert.ToBase64String(stream.ToArray());
        
        return base64;
    }
}
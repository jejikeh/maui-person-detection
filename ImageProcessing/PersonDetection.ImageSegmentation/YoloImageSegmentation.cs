using Microsoft.Extensions.Options;
using PersonDetection.ImageSegmentation.Model;
using PersonDetection.ImageSegmentation.Model.Data.Output;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;

namespace PersonDetection.ImageSegmentation.ModelConverter;

public class YoloImageSegmentation(IOptions<ModelLoaderOptions> _options)
{
    private readonly Instance _quantizedInstance = new Instance(_options.Value.QuantizedModelPath);
    private readonly Instance _unquantizedInstance = new Instance(_options.Value.UnQuantizedModelPath);
    
    public async Task<string> SegmentAsync(string base64Image, ModelType modelType)
    {
        var image = Image.Load<Rgb24>(Convert.FromBase64String(base64Image));
        
        var segmentation = ProperInstance(modelType).Predict(image);
        var output = segmentation.PlotImage(image);
        
        var stream = new MemoryStream();
        await output.SaveAsPngAsync(stream);
        var base64 = Convert.ToBase64String(stream.ToArray());
        
        return base64;
    }
    
    public Segmentation CalculateSegmentation(ref byte[] base64Image, ModelType modelType)
    {
        var image = Image.Load<Rgb24>(base64Image);
        var segmentation = ProperInstance(modelType).Predict(image);

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

    public Instance ProperInstance(ModelType modelType)
    {
        return modelType switch
        {
            ModelType.UnQuantized => _unquantizedInstance!,
            ModelType.Quantized => _quantizedInstance!,
            _ => throw new ArgumentOutOfRangeException(nameof(modelType), modelType, null)
        };
    }
}
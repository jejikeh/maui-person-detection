using Neural.Onnx.Models.Yolo5.Tasks.BoxPredictionsToImage;
using Neural.Onnx.Models.Yolo5.Tasks.ImageToBoxPredictions;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;

namespace Neural.Onnx.Pipelines.Yolo5;

public class Yolo5ImagePlainPipeline : Yolo5ImagePipeline
{
    public async Task<BoxPredictionsToImageTask?> RunTransparentAsync(ImageToBoxPredictionsTask task)
    {
        if (!ClustersInitialized())
        {
            return null;
        }

        var predictions = await Yolo5Cluster!.RunAsync(task);
        
        if (predictions is null)
        {
            return null;
        }
        
        var image = await ImageBoxPainterCluster!.RunAsync(new BoxPredictionsToImageTask(predictions));
        
        return image;
    }
    
    public async Task<BoxPredictionsToImageTask?> RunAsync(ImageToBoxPredictionsTask task)
    {
        if (!ClustersInitialized())
        {
            return null;
        }

        var predictions = await Yolo5Cluster!.RunAsync(task);
        
        if (predictions is null)
        {
            return null;
        }
        
        var image = await ImageBoxPainterCluster!.RunAsync(new BoxPredictionsToImageTask(task.TypedInput.Image, predictions));
        
        return image;
    }
    
    public async Task<string?> RunAsync(string base64Image)
    {
        if (!ClustersInitialized())
        {
            return null;
        }
        
        var image = ConvertStringToImage(base64Image);
        var yoloTask = new ImageToBoxPredictionsTask(image);

        var predictions = await Yolo5Cluster!.RunAsync(yoloTask);
        
        if (predictions is null)
        {
            return null;
        }
        
        var outputImage = await ImageBoxPainterCluster!.RunAsync(new BoxPredictionsToImageTask(yoloTask.TypedInput.Image, predictions));

        if (outputImage?.TypedOutput.Image is null)
        {
            return null;
        }
        
        return await ConvertImageToStringAsync(outputImage.TypedOutput.Image);
    }
    
    private static Image<Rgba32> ConvertStringToImage(string base64)
    {
        return Image.Load<Rgba32>(Convert.FromBase64String(base64));
    }
    
    private static async Task<string> ConvertImageToStringAsync(Image image)
    {
        var stream = new MemoryStream();
        
        await image.SaveAsPngAsync(stream);
        
        var base64 = Convert.ToBase64String(stream.ToArray());
        
        return base64;
    }
}
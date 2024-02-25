using Neural.Onnx.Models.Yolo5.Tasks.ImageToBoxPredictions;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;

namespace Neural.Tests.Onnx.Utils;

public static class DataProvider
{
    public static Image<Rgba32>[] LoadImages(string path)
    {
        var images = Directory
            .GetFiles(path)
            .Select(Image.Load<Rgba32>)
            .ToArray();
        
        return images;
    }
    
    public static Image<Rgba32>[] LoadImages(string[] paths)
    {
        var images = paths
            .Select(Image.Load<Rgba32>)
            .ToArray();
        
        return images;
    }
    
    public static Image<Rgba32> LoadRandomImage(string path)
    {
        var images = Directory
            .GetFiles(path)
            .Select(Image.Load<Rgba32>)
            .ToArray();
        
        var randomImage = images[Random.Shared.Next(images.Length)];
        
        return randomImage;
    }
    
    public static Image<Rgba32> LoadRandomImage(string[] path)
    {
        var randomPath = path[Random.Shared.Next(path.Length)];
        
        var randomImage = Image.Load<Rgba32>(randomPath);
        
        return randomImage;
    }

    public static List<ImageToBoxPredictionsTask> LoadImageToBoxPredictionsTasks(string imagesPath)
    {
        var images = LoadImages(imagesPath);

        var tasks = images
            .Select(image => new ImageToBoxPredictionsTask(image))
            .ToList();

        return tasks;
    }
    
    public static ImageToBoxPredictionsTask LoadImageToBoxPredictionsTask(string imagesPath)
    {
        var image = LoadRandomImage(imagesPath);

        var task = new ImageToBoxPredictionsTask(image);

        return task;
    }
    
    public static ImageToBoxPredictionsTask LoadImageToBoxPredictionsTask(string[] imagesPath)
    {
        var image = LoadRandomImage(imagesPath);

        var task = new ImageToBoxPredictionsTask(image);

        return task;
    }
}
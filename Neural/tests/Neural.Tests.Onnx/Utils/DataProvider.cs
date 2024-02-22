using Neural.Onnx.Tasks.ImageToBoxPredictions;
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

    public static List<ImageToBoxPredictionsYolo5Task> LoadImageToBoxPredictionsTasks(string imagesPath)
    {
        var images = LoadImages(imagesPath);

        var tasks = images
            .Select(image => new ImageToBoxPredictionsYolo5Task(image))
            .ToList();

        return tasks;
    }
    
    public static ImageToBoxPredictionsYolo5Task LoadImageToBoxPredictionsTask(string imagesPath)
    {
        var image = LoadRandomImage(imagesPath);

        var task = new ImageToBoxPredictionsYolo5Task(image);

        return task;
    }
    
    public static ImageToBoxPredictionsYolo5Task LoadImageToBoxPredictionsTask(string[] imagesPath)
    {
        var image = LoadRandomImage(imagesPath);

        var task = new ImageToBoxPredictionsYolo5Task(image);

        return task;
    }
}
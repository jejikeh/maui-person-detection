using Neural.Onnx.Models.Yolo5.Tasks.ImageToBoxPredictions;
using PersonDetection.Backend.Application.Common.Exceptions;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;

namespace PersonDetection.Backend.Application.Services.Implementations;

public class PhotoProcessingService(INeuralService _neuralService) : IPhotoProcessingService
{
    public async Task<string> ProcessPhotoAsync(string base64Image)
    {
        var image = ConvertStringToImage(base64Image);

        var yoloTask = new ImageToBoxPredictionsTask(image);

        var processedPhoto = await _neuralService.Yolo5ImagePlainPipeline.RunAsync(yoloTask);

        if (processedPhoto?.TypedInput.InputImage is null)
        {
            throw new InvalidPhotoException();
        }
        
        var base64 = await ConvertImageToStringAsync(processedPhoto.TypedInput.InputImage!);
        
        return base64;
    }

    public IAsyncEnumerable<string> ProcessPhotosStreamAsync(IAsyncEnumerable<string> photos)
    {
        return _neuralService.Yolo5ImageStreamPipeline.RunAsync(photos);
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
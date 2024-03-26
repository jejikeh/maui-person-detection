using Neural.Onnx.Models.Yolo5.Tasks.ImageToBoxPredictions;
using Neural.Onnx.Tasks.ImageToSegmentation;
using PersonDetection.Backend.Application.Common.Models;
using PersonDetection.Backend.Infrastructure.Services;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;

namespace PersonDetection.Backend.Application.Services.Implementations;

public class PhotoProcessingService(IOnnxNeuralService _onnxNeuralService, IImageBucketService imageBucketService) : IPhotoProcessingService
{
    public async Task<string> ProcessPhotoAsync(string base64Image)
    {
        var image = ConvertStringToImage(base64Image);
        
        var yoloTask = new ImageToBoxPredictionsTask(image);
        var predictions = await _onnxNeuralService.Yolo5PlainImageProcessing(yoloTask);

        var base64OutputImage = await ConvertImageToStringAsync(predictions.TypedInput.InputImage);

        return base64OutputImage;
    }

    public void RunInBackground(string photo, OnnxModelType modelType, Func<string, Task> handlePipelineCompleteAsync)
    {
        switch (modelType)
        {
            case OnnxModelType.Yolo5:
            {
                _onnxNeuralService.Yolo5ImageStreamRunInBackground(photo, handlePipelineCompleteAsync);
                
                break;
            }
            case OnnxModelType.Yolo8:
            {
                _onnxNeuralService.Yolo8ImageStreamRunInBackground(photo, handlePipelineCompleteAsync);
                
                break;
            }
            default:
                throw new ArgumentOutOfRangeException();
        }
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
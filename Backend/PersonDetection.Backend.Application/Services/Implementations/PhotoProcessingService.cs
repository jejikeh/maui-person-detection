using Neural.Onnx.Models.Yolo5.Tasks.ImageToBoxPredictions;
using Neural.Onnx.Tasks.ImageToSegmentation;
using PersonDetection.Backend.Application.Common.Models;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using Tensorflow;

namespace PersonDetection.Backend.Application.Services.Implementations;

public class PhotoProcessingService(
    IOnnxNeuralService _onnxNeuralService, 
    ModelTypeProvider _modelTypeProvider) : IPhotoProcessingService
{
    public async Task<string> ProcessPhotoAsync(string base64Image)
    {
        var base64OutputImage = await PlainImageProcessing(base64Image);
        
        return base64OutputImage;
    }
    
    public void RunInBackground(string photo, Func<string, Task> handlePipelineCompleteAsync)
    {
        switch (_modelTypeProvider.ModelType)
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

    private async Task<string> PlainImageProcessing(string base64Image)
    {
        var image = ConvertStringToImage(base64Image);

        string base64OutputImage;

        switch (_modelTypeProvider.ModelType)
        {
            case OnnxModelType.Yolo5:
            {
                var yoloTask = new ImageToBoxPredictionsTask(image);
                var predictions = await _onnxNeuralService.Yolo5PlainImageProcessing(yoloTask);

                base64OutputImage = await ConvertImageToStringAsync(predictions.TypedInput.InputImage);

                break;
            }
            case OnnxModelType.Yolo8:
            {
                var yoloTask = new ImageToSegmentationTask(image);
                var segmentation = await _onnxNeuralService.Yolo8PlainImageProcessing(yoloTask);

                base64OutputImage = await ConvertImageToStringAsync(segmentation.TypedInput.InputImage);

                break;
            }
            default:
                throw new InvalidArgumentError();
        }

        return base64OutputImage;
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
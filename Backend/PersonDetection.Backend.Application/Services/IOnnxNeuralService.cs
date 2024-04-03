using Neural.Onnx.Models.Yolo5.Tasks.BoxPredictionsToImage;
using Neural.Onnx.Models.Yolo5.Tasks.ImageToBoxPredictions;

namespace PersonDetection.Backend.Application.Services;

public interface IOnnxNeuralService
{
    public Task<BoxPredictionsToImageTask> Yolo5PlainImageProcessingAsync(ImageToBoxPredictionsTask yoloTask);
    public void Yolo5ImageStreamRunInBackground(string photo, Func<string, Task> handlePipelineCompleteAsync);
    public void Yolo8ImageStreamRunInBackground(string photo, Func<string, Task> handlePipelineCompleteAsync);
}
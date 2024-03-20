using Neural.Onnx.Models.Yolo5.Tasks.BoxPredictionsToImage;
using Neural.Onnx.Models.Yolo5.Tasks.ImageToBoxPredictions;
using Neural.Onnx.Models.Yolo8.SegmentationsPredictionsToImage;
using Neural.Onnx.Tasks.ImageToSegmentation;

namespace PersonDetection.Backend.Application.Services;

public interface IOnnxNeuralService
{
    public Task<BoxPredictionsToImageTask> Yolo5PlainImageProcessing(ImageToBoxPredictionsTask yoloTask);

    public Task<BoxPredictionsToImageTask> Yolo5PlainTransparentImageProcessing(ImageToBoxPredictionsTask yoloTask);
    public void Yolo5ImageStreamRunInBackground(string photo, Func<string, Task> handlePipelineCompleteAsync);

    public Task<SegmentationPredictionsToImageTask> Yolo8PlainImageProcessing(ImageToSegmentationTask yoloTask);
    public void Yolo8ImageStreamRunInBackground(string photo, Func<string, Task> handlePipelineCompleteAsync);
}
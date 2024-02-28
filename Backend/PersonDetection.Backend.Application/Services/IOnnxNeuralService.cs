using Neural.Onnx.Models.Yolo5.Tasks.BoxPredictionsToImage;
using Neural.Onnx.Models.Yolo5.Tasks.ImageToBoxPredictions;
using Neural.Onnx.Models.Yolo8.SegmentationsPredictionsToImage;
using Neural.Onnx.Pipelines.Yolo5;
using Neural.Onnx.Pipelines.Yolo8;
using Neural.Onnx.Tasks.ImageToSegmentation;

namespace PersonDetection.Backend.Application.Services;

public interface IOnnxNeuralService
{
    public Yolo5ImagePlainPipeline Yolo5ImagePlainPipeline { get; }
    public Yolo5ImageStreamPipeline Yolo5ImageStreamPipeline { get; }

    public Yolo8ImagePlainPipeline Yolo8ImagePlainPipeline { get; }
    public Yolo8ImageStreamPipeline Yolo8ImageStreamPipeline { get; }

    public Task<SegmentationPredictionsToImageTask> Yolo8PlainImageProcessing(ImageToSegmentationTask yoloTask);
    public Task<BoxPredictionsToImageTask> Yolo5PlainImageProcessing(ImageToBoxPredictionsTask yoloTask);
    public void Yolo8ImageStreamRunInBackground(string photo, Func<string, Task> handlePipelineCompleteAsync);
    public void Yolo5ImageStreamRunInBackground(string photo, Func<string, Task> handlePipelineCompleteAsync);
}
using Neural.Core;
using Neural.Onnx.Models.Yolo5.Tasks.BoxPredictionsToImage;
using Neural.Onnx.Models.Yolo5.Tasks.ImageToBoxPredictions;
using Neural.Onnx.Models.Yolo8.SegmentationsPredictionsToImage;
using Neural.Onnx.Pipelines.Yolo5;
using Neural.Onnx.Pipelines.Yolo8;
using Neural.Onnx.Tasks.ImageToSegmentation;
using PersonDetection.Backend.Application.Common.Exceptions;

namespace PersonDetection.Backend.Application.Services.Implementations;

public class OnnxNeuralService : IOnnxNeuralService
{
    private readonly Yolo5ImagePlainPipeline _yolo5ImagePlainPipeline;
    private readonly Yolo5ImageStreamPipeline _yolo5ImageStreamPipeline;
    private readonly Yolo8ImagePlainPipeline _yolo8ImagePlainPipeline;
    private readonly Yolo8ImageStreamPipeline _yolo8ImageStreamPipeline;

    public OnnxNeuralService(NeuralHub _neuralHub)
    {
        _yolo5ImagePlainPipeline = _neuralHub.ExtractPipeline<Yolo5ImagePlainPipeline>() 
                                   ?? throw new NullReferenceException(nameof(_yolo5ImageStreamPipeline));
        
        _yolo5ImageStreamPipeline = _neuralHub.ExtractPipeline<Yolo5ImageStreamPipeline>() 
                                    ?? throw new NullReferenceException(nameof(_yolo5ImageStreamPipeline));
        
        _yolo8ImagePlainPipeline = _neuralHub.ExtractPipeline<Yolo8ImagePlainPipeline>()
                                   ?? throw new NullReferenceException(nameof(_yolo8ImagePlainPipeline));
        
        _yolo8ImageStreamPipeline = _neuralHub.ExtractPipeline<Yolo8ImageStreamPipeline>()
                                    ?? throw new NullReferenceException(nameof(_yolo5ImageStreamPipeline));
    }

    public async Task<SegmentationPredictionsToImageTask> Yolo8PlainImageProcessing(ImageToSegmentationTask yoloTask)
    {
        var processedPhoto = await _yolo8ImagePlainPipeline.RunAsync(yoloTask);

        if (processedPhoto?.TypedInput.InputImage is null)
        {
            throw new InvalidPhotoException();
        }

        return processedPhoto;
    }
    
    public void Yolo8ImageStreamRunInBackground(string photo, Func<string, Task> handlePipelineCompleteAsync)
    {
        _yolo8ImageStreamPipeline.RunInBackground(photo, handlePipelineCompleteAsync);
    }

    public async Task<BoxPredictionsToImageTask> Yolo5PlainImageProcessing(ImageToBoxPredictionsTask yoloTask)
    {
        var processedPhoto = await _yolo5ImagePlainPipeline.RunAsync(yoloTask);

        if (processedPhoto?.TypedInput.InputImage is null)
        {
            throw new InvalidPhotoException();
        }

        return processedPhoto;
    }

    public async Task<BoxPredictionsToImageTask> Yolo5PlainTransparentImageProcessing(ImageToBoxPredictionsTask yoloTask)
    {
        var processedPhoto = await _yolo5ImagePlainPipeline.RunTransparentAsync(yoloTask);

        if (processedPhoto?.TypedInput.InputImage is null)
        {
            throw new InvalidPhotoException();
        }

        return processedPhoto;
    }
    
    public void Yolo5ImageStreamRunInBackground(string photo, Func<string, Task> handlePipelineCompleteAsync)
    {
        _yolo5ImageStreamPipeline.RunInBackground(photo, handlePipelineCompleteAsync);
    }
}
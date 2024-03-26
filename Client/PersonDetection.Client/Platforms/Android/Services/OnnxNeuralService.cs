// To fix IDE errors.
#if ANDROID
using Neural.Core;
using Neural.Onnx.Pipelines.Yolo5;

namespace PersonDetection.Client.Platforms.Android.Services;

public class OnnxNeuralService
{
    private readonly Yolo5ImagePlainPipeline _yolo5ImagePlainPipeline;

    public OnnxNeuralService(NeuralHub _neuralHub)
    {
        _yolo5ImagePlainPipeline = _neuralHub.ExtractPipeline<Yolo5ImagePlainPipeline>() 
                                   ?? throw new NullReferenceException(nameof(_yolo5ImagePlainPipeline));
    }
    
    public async Task<string?> ProcessPhotoAsync(string base64Image)
    {
        var outputImage = await _yolo5ImagePlainPipeline.RunAsync(base64Image);

        return outputImage;
    }
}
#endif

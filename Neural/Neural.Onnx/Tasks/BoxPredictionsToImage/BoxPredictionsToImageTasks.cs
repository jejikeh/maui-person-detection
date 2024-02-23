using Neural.Core.Models;
using Neural.Onnx.Tasks.ImageToBoxPredictions;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;

namespace Neural.Onnx.Tasks.BoxPredictionsToImage;

public class BoxPredictionsToImageTasks(ImageToBoxPredictionsTask _imageToBoxPredictionsTask) : IModelTask
{
    public IModelInput Input { get; set; } = new BoxPredictionsInput(
        _imageToBoxPredictionsTask.Yolo5ImageInput().Image,
        _imageToBoxPredictionsTask.Yolo5BoxPredictionsOutput().Predictions);
    
    public IModelOutput Output { get; set; } = new ImageOutput();
    
    public event Action<ImageOutput>? OnModelTaskComplete;
    
    public BoxPredictionsInput BoxPredictionsInput()
    {
        return (BoxPredictionsInput)Input;
    }
    
    public ImageOutput ImageOutput()
    {
        return (ImageOutput)Output;
    }
    
    public void SetOutput(IModel model, object value)
    {
        Output.Set(value);
        
        OnModelTaskComplete?.Invoke(ImageOutput());
    }
}
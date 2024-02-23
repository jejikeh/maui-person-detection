using Neural.Core.Models;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;

namespace Neural.Onnx.Tasks.ImageToSegmentation;

public class ImageToSegmentationTask(Image<Rgba32> _image, string[] _inputNames) : IModelTask
{
    public IModelInput Input { get; set; } = new Yolo8ImageInput(_image, _inputNames);
    public IModelOutput Output { get; set; }
    
    public Yolo8ImageInput Yolo8ImageInput()
    {
        return (Yolo8ImageInput)Input;
    }
    
    public void SetOutput(IModel model, object value)
    {
        Output.Set(value);
    }
}
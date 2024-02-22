using Microsoft.ML.OnnxRuntime;
using Neural.Core.Models;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;

namespace Neural.Onnx.Tasks.ImageToBoxPredictions;

public class ImageToBoxPredictionsYolo5Task(Image<Rgba32> _image) : IModelTask
{
    public IModelInput Input { get; set; } = new Yolo5ImageInput(_image);
    public IModelOutput Output { get; set; } = new Yolo5BoxPredictionsOutput();
    
    public Yolo5ImageInput Yolo5ImageInput()
    {
        return (Yolo5ImageInput)Input;
    }
    
    public Yolo5BoxPredictionsOutput Yolo5BoxPredictionsOutput()
    {
        return (Yolo5BoxPredictionsOutput)Output;
    }

    public void SetOutput(IModel model, object value)
    {
        Output.Set(value);
    }
}
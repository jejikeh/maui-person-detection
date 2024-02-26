using Neural.Core.Models;
using Neural.Defaults.Models;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;

namespace Neural.Onnx.Models.Yolo5.Tasks.ImageToBoxPredictions;

public class ImageToBoxPredictionsTask : ModelTask<Yolo5ImageInput, Yolo5BoxPredictionsOutput>
{
    public ImageToBoxPredictionsTask(Image<Rgba32> _image)
    {
        Input = new Yolo5ImageInput(_image);
    }
    
    public ImageToBoxPredictionsTask(string base64Image)
    {
        Input = new Yolo5ImageInput(Image.Load<Rgba32>(Convert.FromBase64String(base64Image)));
    }

    public sealed override IModelInput Input { get; set; }
    public override IModelOutput Output { get; set; } = new Yolo5BoxPredictionsOutput();
}
using Neural.Core.Models;
using Neural.Defaults.Models;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;

namespace Neural.Onnx.Models.Yolo5.Tasks.ImageToBoxPredictions;

public class ImageToBoxPredictionsTask(Image<Rgba32> _image) : ModelTask<Yolo5ImageInput, Yolo5BoxPredictionsOutput>
{
    public override IModelInput Input { get; set; } = new Yolo5ImageInput(_image);
    public override IModelOutput Output { get; set; } = new Yolo5BoxPredictionsOutput();
}
using Neural.Core.Models;
using Neural.Defaults.Models;
using Neural.Onnx.Models.Yolo5.Tasks;
using Neural.Onnx.Models.Yolo8.ImageToSegmentation;
using Neural.Onnx.Models.Yolo8.Specifications;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;

namespace Neural.Onnx.Tasks.ImageToSegmentation;

public class ImageToSegmentationTask : ModelTask<Yolo8ImageInput, Yolo8SegmentationOutput>
{
    public sealed override IModelInput Input { get; set; }
    public override IModelOutput Output { get; set; } = new Yolo8SegmentationOutput();
    
    public ImageToSegmentationTask(Image<Rgba32> image)
    {
        Input = new Yolo8ImageInput(image);
    }
    
    public ImageToSegmentationTask(string base64Image)
    {
        Input = new Yolo8ImageInput(Image.Load<Rgba32>(Convert.FromBase64String(base64Image)));
    }
}
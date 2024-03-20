using Neural.Core.Models;
using Neural.Defaults.Models;
using Neural.Onnx.Models.Yolo5.Specifications;
using Neural.Onnx.Models.Yolo5.Tasks.ImageToBoxPredictions;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;

namespace Neural.Onnx.Models.Yolo5.Tasks.BoxPredictionsToImage;

public class BoxPredictionsToImageTask : ModelTask<BoxPredictionsInput, ImageOutput>
{
    public sealed override IModelInput Input { get; set; }
    public override IModelOutput Output { get; set; } = new ImageOutput();
    
    public BoxPredictionsToImageTask(ImageToBoxPredictionsTask imageToBoxPredictionsTask)
    {
        Input = new BoxPredictionsInput(
            new Image<Rgba32>(Yolo5Specification.InputSize.Width, Yolo5Specification.InputSize.Height, Color.Transparent),
            imageToBoxPredictionsTask.TypedOutput.Predictions);
    }
    
    public BoxPredictionsToImageTask(Image<Rgba32> image, ImageToBoxPredictionsTask imageToBoxPredictionsTask)
    {
        Input = new BoxPredictionsInput(
            image,
            imageToBoxPredictionsTask.TypedOutput.Predictions);
    }
}
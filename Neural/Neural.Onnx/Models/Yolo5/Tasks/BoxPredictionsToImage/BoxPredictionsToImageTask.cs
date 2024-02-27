using Neural.Core.Models;
using Neural.Defaults.Models;
using Neural.Onnx.Models.Yolo5.Specifications;
using Neural.Onnx.Models.Yolo5.Tasks.ImageToBoxPredictions;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;

namespace Neural.Onnx.Models.Yolo5.Tasks.BoxPredictionsToImage;

public class BoxPredictionsToImageTask(ImageToBoxPredictionsTask _imageToBoxPredictionsTask) 
    : ModelTask<BoxPredictionsInput, ImageOutput>
{
    public override IModelInput Input { get; set; } = new BoxPredictionsInput(
        new Image<Rgba32>(Yolo5Specification.InputSize.Width, Yolo5Specification.InputSize.Height, Color.Transparent),
        _imageToBoxPredictionsTask.TypedOutput.Predictions);
    
    public override IModelOutput Output { get; set; } = new ImageOutput();
}
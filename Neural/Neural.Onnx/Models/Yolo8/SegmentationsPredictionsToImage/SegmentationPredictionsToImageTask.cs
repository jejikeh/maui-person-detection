using Neural.Core.Models;
using Neural.Defaults.Models;
using Neural.Onnx.Models.Yolo5.Specifications;
using Neural.Onnx.Models.Yolo5.Tasks;
using Neural.Onnx.Models.Yolo5.Tasks.BoxPredictionsToImage;
using Neural.Onnx.Tasks.ImageToSegmentation;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;

namespace Neural.Onnx.Models.Yolo8.SegmentationsPredictionsToImage;

public class SegmentationPredictionsToImageTask(ImageToSegmentationTask _imageToSegmentationTask) 
    : ModelTask<SegmentationPredictionsInput, ImageOutput>
{
    public override IModelInput Input { get; set; } = new SegmentationPredictionsInput(
        new Image<Rgba32>(Yolo5Specification.InputSize.Width, Yolo5Specification.InputSize.Height, Color.Transparent),
        _imageToSegmentationTask.TypedOutput.Predictions);
    
    public override IModelOutput Output { get; set; } = new ImageOutput();
}
using Neural.Core.Models;
using Neural.Defaults.Models;
using Neural.Onnx.Models.Yolo5.Tasks.ImageToBoxPredictions;

namespace Neural.Onnx.Models.Yolo5.Tasks.BoxPredictionsToImage;

public class BoxPredictionsToImageTasks(ImageToBoxPredictionsTask _imageToBoxPredictionsTask) 
    : ModelTask<BoxPredictionsInput, ImageOutput>
{
    public override IModelInput Input { get; set; } = new BoxPredictionsInput(
        _imageToBoxPredictionsTask.TypedInput.Image,
        _imageToBoxPredictionsTask.TypedOutput.Predictions);
    
    public override IModelOutput Output { get; set; } = new ImageOutput();
}
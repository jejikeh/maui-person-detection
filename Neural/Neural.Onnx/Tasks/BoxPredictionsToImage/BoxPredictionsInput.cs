using Neural.Core.Models;
using Neural.Onnx.Models.Yolo5.Specifications;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;

namespace Neural.Onnx.Tasks.BoxPredictionsToImage;

public class BoxPredictionsInput(Image<Rgba32> _inputImage, List<Yolo5Prediction>? _predictions) : IModelInput
{
    public Image<Rgba32> InputImage => _inputImage;
    public List<Yolo5Prediction> Predictions => _predictions ?? new List<Yolo5Prediction>();
}
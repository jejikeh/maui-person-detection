using Neural.Core.Models;
using Neural.Onnx.Models.Yolo5.Specifications;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;

namespace Neural.Onnx.Models.Yolo5.Tasks.BoxPredictionsToImage;

public class BoxPredictionsInput(Image<Rgba32> _inputImage, IEnumerable<Yolo5Prediction>? _predictions) : IModelInput
{
    public Image<Rgba32> InputImage => _inputImage;
    public IEnumerable<Yolo5Prediction> Predictions => _predictions ?? new List<Yolo5Prediction>();
}
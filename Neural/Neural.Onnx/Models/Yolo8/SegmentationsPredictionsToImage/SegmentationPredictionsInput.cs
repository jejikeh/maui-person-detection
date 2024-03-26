using Neural.Core.Models;
using Neural.Onnx.Models.Yolo5.Specifications;
using Neural.Onnx.Models.Yolo8.Specifications;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;

namespace Neural.Onnx.Models.Yolo8.SegmentationsPredictionsToImage;

public class SegmentationPredictionsInput(Image<Rgba32> _inputImage, IEnumerable<SegmentationBoundBox>? _predictions) : IModelInput
{
    public Image<Rgba32> InputImage => _inputImage;
    public IEnumerable<SegmentationBoundBox> Predictions => _predictions ?? new List<SegmentationBoundBox>();
}
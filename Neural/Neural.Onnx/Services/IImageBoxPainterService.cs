using Neural.Onnx.Models.Yolo5.Specifications;
using Neural.Onnx.Models.Yolo8.Specifications;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;

namespace Neural.Onnx.Services;

public interface IImageBoxPainterService
{
    public void PaintPredictions(Image<Rgba32> image, IEnumerable<Yolo5Prediction> predictions);
    public void PaintPersonPredictions(Image<Rgba32> image, IEnumerable<SegmentationBoundBox> predictions);
}
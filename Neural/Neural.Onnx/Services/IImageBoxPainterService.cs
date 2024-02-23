using Neural.Onnx.Models.Yolo5.Specifications;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;

namespace Neural.Onnx.Services;

public interface IImageBoxPainterService
{
    public void PaintPredictions(Image<Rgba32> image, List<Yolo5Prediction> predictions);
}
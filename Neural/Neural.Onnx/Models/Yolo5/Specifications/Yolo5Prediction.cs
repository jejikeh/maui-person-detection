using SixLabors.ImageSharp;

namespace Neural.Onnx.Models.Yolo5.Specifications;

public record Yolo5Prediction(YoloClass Class, float Score, RectangleF Rectangle);
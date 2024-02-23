using SixLabors.ImageSharp;

namespace Neural.Onnx.Models.Yolo5.Specifications;

public record Yolo5Prediction(Yolo5Class Class, float Score, RectangleF Rectangle);
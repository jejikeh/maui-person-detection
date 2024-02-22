using Neural.Onnx.Models.Yolo5.Specifications;
using SixLabors.ImageSharp;

namespace Neural.Onnx.Tasks.ImageToBoxPredictions;

public record Yolo5Prediction(Yolo5Class Class, float Score, RectangleF Rectangle);
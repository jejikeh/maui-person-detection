using SixLabors.ImageSharp;

namespace PersonDetection.ImageProcessing.Model;

public record YoloPrediction(YoloLabel Label, float Score, RectangleF Rectangle);

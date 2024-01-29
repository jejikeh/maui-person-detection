using SixLabors.ImageSharp;

namespace PersonDetection.ImageProcessing.Model;

/// <summary>
/// Object prediction.
/// </summary>
public record YoloPrediction(YoloLabel Label, float Score, RectangleF Rectangle);

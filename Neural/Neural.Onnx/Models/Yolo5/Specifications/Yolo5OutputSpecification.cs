using System.Collections.Concurrent;
using System.Diagnostics.CodeAnalysis;
using Microsoft.ML.OnnxRuntime.Tensors;
using SixLabors.ImageSharp;

namespace Neural.Onnx.Models.Yolo5.Specifications;

public static class Yolo5OutputSpecification
{
    public static int BatchSize { get; set; } = 0;
    public static int ConfidenceLayer { get; set; } = 4;
    public static int Dimensions { get; set; } = 85;
    
    public static float OverlapThreshold { get; set; } = 0.5f;
    public static float ConfidenceThreshold { get; set; } = 0.2f;
    public static float DimensionValueThreshold { get; set; } = 0.2f;
    
    public const int XLayer = 0;
    public const int YLayer = 1;
    public const int WidthLayer = 2;
    public const int HeightLayer = 3;
    
    public const int LabelDimensionOffset = 5;
}
using Neural.Core.Models;

namespace Neural.Tests.Common.Mocks.Options;

public class Yolo8QuantizedOptions : IModelOptions
{
    public string Path { get; } = ModelPath;
    public static string ModelPath = $"{Constants.ModelPaths}/Yolo8/yolov8n-seg-quantized.onnx";
}
using Neural.Core.Models;

namespace Neural.Core.Tests.Mocks.Options;

public class Yolo8QuantizedOptions : IModelOptions
{
    public string Path { get; } = $"{Constants.ModelPaths}/Yolo8/yolov8n-seg-quantized.onnx";
}
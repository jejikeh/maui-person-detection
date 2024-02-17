using Neural.Core.Models;

namespace Neural.Core.Tests.Mocks.Options;

public class Yolo5Options : IModelOptions
{
    public string Path { get; } = $"{Constants.ModelPaths}/Yolo5/yolov5n.onnx";
}
using Neural.Core.Models;

namespace Neural.Tests.Common.Mocks.Options;

public class Yolo5Options : IModelOptions
{
    public string Path { get; } = ModelPath;
    public static string ModelPath = $"{Constants.ModelPaths}/Yolo5/yolov5n.onnx";
}
namespace Neural.Tests.Common.Mocks;

public static class Constants
{
    private const string _modelPaths = "TestingData/Models";
    public const string Yolo5ModelPath = $"{_modelPaths}/Yolo5/yolov5n.onnx";
    public const string Yolo8ModelPath = $"{_modelPaths}/Yolo8/yolov8n-seg-quantize.onnx";
}
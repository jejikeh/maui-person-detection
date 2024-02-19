namespace Neural.Tests.Common.Mocks;

public static class Constants
{
    public const string ModelPaths = "TestingData/Models";
    public static string Yolo5ModelPath = $"{ModelPaths}/Yolo5/yolov5n.onnx";
    public static string Yolo8ModelPath = $"{Constants.ModelPaths}/Yolo8/yolov8n-seg-quantized.onnx";
}
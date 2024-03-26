namespace Neural.Tests.Common.Mocks;

public static class Paths
{
    private const string _testingPath = "TestingData";
    
    private const string _modelPaths = $"{_testingPath}/Models";
    public const string Yolo5ModelPath = $"{_modelPaths}/Yolo5/yolov5n.onnx";
    public const string Yolo8ModelPath = $"{_modelPaths}/Yolo8/yolov8n-seg-quantize.onnx";
    
    private const string _imagesPath = $"{_testingPath}/Images";
    public const string ValidImages = $"{_imagesPath}/Valid";
    public const string InvalidImages = $"{_imagesPath}/Invalid";

    public static readonly string[] ValidImagesWithObjectsPaths = [
        $"{ValidImages}/7.jpg",
        $"{ValidImages}/6.jpg",
    ];
}
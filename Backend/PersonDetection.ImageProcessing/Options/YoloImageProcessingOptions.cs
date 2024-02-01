namespace PersonDetection.ImageProcessing.Options;

public class YoloImageProcessingOptions
{
    public string WeightsDirectory { get; set; }
    public string WeightFile { get; set; }
    public string WeightsPath => WeightsDirectory + WeightFile + ".onnx";
    public int FontSize { get; set; }
}
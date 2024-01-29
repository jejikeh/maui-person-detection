namespace PersonDetection.ImageProcessing.Configuration;

public interface IImageProcessingConfiguration
{
    public string WeightsDirectory { get; set; }
    public WeightType WeightType { get; set; }
    public string WeightsPath => WeightsDirectory + WeightType.ToString().ToLower() + ".onnx";
    public int FontSize { get; set; }
}

public enum WeightType
{
    YoloV5N,
    YoloV5S
}
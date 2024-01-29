using PersonDetection.Client.Infrastructure.Common;
using PersonDetection.ImageProcessing.Configuration;

namespace PersonDetection.Client.Configuration;

public class ClientConfiguration : IInfrastructureConfiguration, IImageProcessingConfiguration
{
    // Client
    public bool UseExceptionHandler => true;
    public bool DisplayExceptionDetails => true;
    public PhotoProcessProvider PhotoProcessProvider { get; set; } = PhotoProcessProvider.YoloV5;
    
    // Infrastructure
    public string PhotoProcessUrl { get; set; } = "http://localhost:12532/imagegen";
    public TimeSpan CacheExpirationTime { get; set; } = TimeSpan.FromHours(1);
    public string DatabaseFileName { get; set; } = "PersonDetection.db";
    public string ImageDirectoryName => "Images";

    // Image processing 
    public string WeightsDirectory { get; set; } = "Weights/";
    public WeightType WeightType { get; set; } = WeightType.YoloV5N;
    public int FontSize { get; set; } = 18;
}
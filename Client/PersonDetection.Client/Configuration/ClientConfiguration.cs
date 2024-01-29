using PersonDetection.Client.Infrastructure.Common;
using PersonDetection.ImageProcessing.Configuration;

namespace PersonDetection.Client.Configuration;

public class ClientConfiguration : IInfrastructureConfiguration, IImageProcessingConfiguration
{
    // Client
    public bool UseExceptionHandler => true;
    public bool DisplayExceptionDetails => true;
    
    // Infrastructure
    public string PhotoProcessUrl { get; set; } = "http://localhost:12532/imagegen";
    public TimeSpan CacheExpirationTime { get; set; } = TimeSpan.FromHours(1);
    public string DatabaseFileName { get; set; } = "PersonDetection.db";
    public string ImageDirectoryName => "Images";
    public PhotoProcessProvider PhotoProcessProvider { get; set; } = PhotoProcessProvider.YoloV5;

    // Image processing 
    public string WeightsDirectory { get; set; } = "Weights/";
    public WeightType WeightType { get; set; } = WeightType.YoloV5N;
    public string FontPath { get; set; } = "Inter.ttf";
    public int FontSize { get; set; } = 16;
}


namespace PersonDetection.ImageSegmentation.ModelConverter;

public sealed class ModelLoaderOptions
{
    public string UnQuantizedModelPath { get; set; } = string.Empty;
    public string QuantizedModelPath { get; set; } = string.Empty;
}
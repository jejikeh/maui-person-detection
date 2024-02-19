namespace PersonDetection.Client.Common.Options;

public class ClientOptions
{
    public bool UseExceptionHandler { get; set; }
    public bool DisplayExceptionDetails { get; set; }
    public PhotoProcessProvider PhotoProcessProvider { get; set; } = PhotoProcessProvider.YoloV5;
}
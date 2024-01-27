using PersonDetection.Client.Infrastructure.Common;

namespace PersonDetection.Client.Configuration;

public class ClientConfiguration : IInfrastructureConfiguration
{
    public string PhotoProcessUrl { get; set; } = "http://192.168.100.6:12532/imagegen";
    public TimeSpan CacheExpirationTime { get; set; } = TimeSpan.FromHours(1);
    public string DatabaseFileName { get; set; } = "PersonDetection.db";
    public string ImageDirectoryName { get; } = "Images";
    public bool UseExceptionHandler { get; set; } = true;
    public bool DisplayExceptionDetails { get; set; } = true;
}
namespace PersonDetection.Client.Infrastructure.Common;

public interface IInfrastructureConfiguration
{
    public string PhotoProcessUrl { get; set; }
    public TimeSpan CacheExpirationTime { get; set; }
    public string DatabaseFileName { get; set; }
    
    public string DatabasePath =>
        Path.Combine(FileSystem.AppDataDirectory, DatabaseFileName);
}
namespace PersonDetection.Client.Infrastructure.Common;

public interface IInfrastructureConfiguration
{
    public string PhotoProcessUrl { get; }
    public TimeSpan CacheExpirationTime { get; }
    public string DatabaseFileName { get; }
    public string ImageDirectoryName { get; }
    public string DatabasePath => Path.Combine(FileSystem.AppDataDirectory, DatabaseFileName);
    public string ImageCacheDirectory => Path.Combine(FileSystem.AppDataDirectory, ImageDirectoryName);
}

public enum PhotoProcessProvider
{
    Http,
    YoloV5
}
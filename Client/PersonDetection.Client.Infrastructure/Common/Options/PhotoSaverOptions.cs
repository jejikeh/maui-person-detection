namespace PersonDetection.Client.Infrastructure.Common.Options;

public sealed class PhotoSaverOptions
{
    public string ImageDirectoryName { get; set; } = string.Empty;
    public string ImageCacheDirectory => Path.Combine(FileSystem.AppDataDirectory, ImageDirectoryName);
}
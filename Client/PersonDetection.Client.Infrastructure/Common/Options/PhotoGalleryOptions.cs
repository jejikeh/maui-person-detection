namespace PersonDetection.Client.Infrastructure.Common.Options;

public sealed class PhotoGalleryOptions
{
    public string DatabaseFileName { get; set; } = string.Empty;
    public string DatabasePath => Path.Combine(FileSystem.AppDataDirectory, DatabaseFileName);
}
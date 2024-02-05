using PersonDetection.ImageProcessing.Services;

namespace PersonDetection.Client.Infrastructure.Services;

public class MauiFileSystemStreamProvider : IFileSystemStreamProvider
{
    public Task<Stream> GetFileStream(string path)
    {
        return FileSystem.OpenAppPackageFileAsync(path);
    }
}
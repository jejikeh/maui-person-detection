using Neural.Core.Services;

namespace PersonDetection.Client.Platforms.Android.Services;

public class MauiFileSystemStreamProvider : IFileSystemProvider
{
    public Task<Stream> GetFileStreamAsync(string path)
    {
        return FileSystem.OpenAppPackageFileAsync(path);
    }
}
using PersonDetection.ImageProcessing.Services;

namespace PersonDetection.Backend.Infrastructure.Services;

public class CoreFileSystemStreamProvider : IFileSystemStreamProvider
{
    public Task<Stream> GetFileStream(string path)
    {
        var stream = File.OpenRead(path);
        
        return Task.FromResult((Stream)stream);
    }
}
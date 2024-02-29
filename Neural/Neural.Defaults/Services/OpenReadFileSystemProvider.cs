using Neural.Core.Services;

namespace Neural.Defaults.Services;

public class OpenReadFileSystemProvider : IFileSystemProvider
{
    public Task<Stream> GetFileStreamAsync(string path)
    {
        var stream = File.OpenRead(path);
        
        return Task.FromResult((Stream)stream);
    }
}
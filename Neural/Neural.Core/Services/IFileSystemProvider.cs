namespace Neural.Core.Services;

public interface IFileSystemProvider
{
    public Task<Stream> GetFileStreamAsync(string path);
}
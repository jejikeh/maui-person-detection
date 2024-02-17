using System.IO;
using System.Threading.Tasks;

namespace PersonDetection.ImageProcessing.Services;

public interface IFileSystemStreamProvider
{
    public Task<Stream> GetFileStreamAsync(string path);
}
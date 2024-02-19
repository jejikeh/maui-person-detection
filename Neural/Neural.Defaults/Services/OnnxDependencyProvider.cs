using Neural.Core.Models;
using Neural.Core.Services;
using Neural.Defaults.Common.Options;
using Neural.Defaults.Models;

namespace Neural.Defaults.Services;

public class OnnxDependencyProvider : IDependencyProvider
{
    public Task<IDependencyContainer> CreateContainerAsync<T>(T? options = default)
    {
        if (options is OnnxOptions onnxOptions)
        {
            return CreateWorkerAsync(onnxOptions);
        }
        
        throw new InvalidOperationException($"{nameof(OnnxDependencyProvider)} can only be used with {nameof(OnnxOptions)}");
    }
    
    public async Task<IDependencyContainer> CreateWorkerAsync(OnnxOptions onnxOptions)
    {
        using var modelStream = new MemoryStream();
        
        var stream = await onnxOptions.FileSystemProvider.GetFileStreamAsync(onnxOptions.ModelPath);
        
        await stream.CopyToAsync(modelStream);

        return new OnnxDependencies(modelStream);
    }
}
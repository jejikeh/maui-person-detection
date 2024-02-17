using Neural.Core;
using Neural.Core.Services;
using Neural.Defaults.Services;

namespace Neural.Defaults;

public static class NeuralHubConfiguration
{
    public static NeuralHubBuilder FromDefaults(IFileSystemProvider? fileSystemProvider = null, IModelProvider? modelProvider = null)
    {
        fileSystemProvider ??= new OpenReadFileSystemProvider();
        modelProvider ??= new MemoryStreamModelProvider();
        
        return new NeuralHubBuilder(fileSystemProvider, modelProvider);
    }
}
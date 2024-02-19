using Neural.Core;
using Neural.Core.Services;
using Neural.Defaults.Services;

namespace Neural.Defaults;

public static class NeuralHubConfiguration
{
    public static NeuralHubBuilder FromDefaults(
        IFileSystemProvider? fileSystemProvider = null, 
        IModelProvider? modelProvider = null,
        IClusterProvider? clusterProvider = null,
        IModelWorkerProvider? workerProvider = null)
    {
        fileSystemProvider ??= new OpenReadFileSystemProvider();
        modelProvider ??= new WorkerModelProvider();
        clusterProvider ??= new ClusterProvider();
        workerProvider ??= new OnnxWorkerProvider();
        
        return new NeuralHubBuilder(
            fileSystemProvider, 
            modelProvider, 
            clusterProvider, 
            workerProvider);
    }
}
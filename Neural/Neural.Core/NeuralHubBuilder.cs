using System.Collections.Concurrent;
using Neural.Core.Models;
using Neural.Core.Services;

namespace Neural.Core;

public class NeuralHubBuilder(
    IFileSystemProvider _fileSystemProvider, 
    IModelProvider _modelProvider, 
    IClusterProvider _clusterProvider)
{
    private readonly NeuralHub _neuralHub = new NeuralHub(_clusterProvider);
    private readonly List<Func<IModel>> _modelProviders = new List<Func<IModel>>();
    
    public IFileSystemProvider FileSystemProvider => _fileSystemProvider;
    public IModelProvider ModelProvider => _modelProvider;
    public IClusterProvider ClusterProvider => _clusterProvider;
    
    public NeuralHubBuilder AddModel<TModel, TModelTask, TDependencyContainer>(TDependencyContainer dependencyContainer) 
        where TModel : class, IModel<TModelTask, TDependencyContainer> 
        where TModelTask : IModelTask
        where TDependencyContainer : class, IDependencyContainer
    {
        _modelProviders.Add(() => _modelProvider.Initialize<TModel, TModelTask, TDependencyContainer>(dependencyContainer));
        
        return this;
    }
    
    public NeuralHubBuilder AddModel<TModel, TModelTask>() 
        where TModel : class, IModel<TModelTask>
        where TModelTask : IModelTask
    {
        _modelProviders.Add(_modelProvider.Initialize<TModel, TModelTask>);
        
        return this;
    }
    
    public NeuralHub Build()
    {
        var models = InitializeModels();

        _neuralHub.Models.AddRange(models);
        
        return _neuralHub;
    }

    private ConcurrentBag<IModel> InitializeModels()
    {
        var concurrentBagOfModels = new ConcurrentBag<IModel>();

        Parallel.ForEach(_modelProviders, modelProvider =>
        {
            var model = modelProvider();
            concurrentBagOfModels.Add(model);
        });
        
        return concurrentBagOfModels;
    }
}
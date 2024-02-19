using System.Collections.Concurrent;
using Neural.Core.Models;
using Neural.Core.Services;

namespace Neural.Core;

public class NeuralHubBuilder(
    IFileSystemProvider _fileSystemProvider, 
    IModelProvider _modelProvider, 
    IClusterProvider _clusterProvider,
    IDependencyProvider dependencyProvider)
{
    private readonly NeuralHub _neuralHub = new NeuralHub(_clusterProvider);
    private readonly List<Func<Task<IModel>>> _modelProviders = new List<Func<Task<IModel>>>();
    
    public IFileSystemProvider FileSystemProvider => _fileSystemProvider;
    public IModelProvider ModelProvider => _modelProvider;
    public IClusterProvider ClusterProvider => _clusterProvider;
    public IDependencyProvider DependencyProvider => dependencyProvider;
    
    public NeuralHubBuilder AddModel<TModel, TModelTask, TWorkerOptions>(TWorkerOptions modelPath) 
        where TModel : class, IModel<TModelTask> 
        where TModelTask : IModelTask
    {
        _modelProviders.Add(async () => await _modelProvider.InitializeAsync<TModel, TModelTask, TWorkerOptions>(dependencyProvider, modelPath));
        
        return this;
    }
    
    public NeuralHubBuilder AddModel<TModel, TModelTask, TOptions>(IModelOptions options) 
        where TModel : class, IModel<TModelTask, TOptions>
        where TOptions : class, IModelOptions
        where TModelTask : IModelTask
    {
        _modelProviders.Add(() => _modelProvider.InitializeAsync<TModel, TModelTask, TOptions>(
            dependencyProvider, 
            options as TOptions ?? throw new InvalidOperationException($"{nameof(AddModel)} can only be used with {nameof(IModelOptions)}")));
        
        return this;
    }

    public NeuralHubBuilder AddModel<TModel, TModelTask, TOptions, TWorkerOptions>(TOptions options, TWorkerOptions workerOptions) 
        where TModel : class, IModel<TModelTask, TOptions>
        where TOptions : class, IModelOptions
        where TModelTask : IModelTask
    {
        _modelProviders.Add(() => _modelProvider.InitializeAsync<TModel, TModelTask, TOptions, TWorkerOptions>(dependencyProvider, options, workerOptions));
        
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
            var model = modelProvider().Result;
            concurrentBagOfModels.Add(model);
        });
        
        return concurrentBagOfModels;
    }
}
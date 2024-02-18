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
    private readonly List<Func<Task<IModel>>> _modelProviders = new List<Func<Task<IModel>>>();
    
    public NeuralHubBuilder AddModel<TModel, TModelTask>(string modelPath) 
        where TModel : class, IModel<TModelTask> 
        where TModelTask : IModelTask
    {
        _modelProviders.Add(async () => await _modelProvider.InitializeAsync<TModel, TModelTask>(_fileSystemProvider, modelPath));
        
        return this;
    }

    public NeuralHubBuilder AddModel<TModel, TModelTask, TOptions>(TOptions options) 
        where TModel : class, IModel<TModelTask, TOptions> 
        where TOptions : class, IModelOptions
        where TModelTask : IModelTask
    {
        _modelProviders.Add(() => _modelProvider.InitializeAsync<TModel, TModelTask, TOptions>(_fileSystemProvider, options));
        
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
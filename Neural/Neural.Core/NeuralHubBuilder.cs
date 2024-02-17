using Neural.Core.Models;
using Neural.Core.Services;

namespace Neural.Core;

public class NeuralHubBuilder(IFileSystemProvider _fileSystemProvider, IModelProvider _modelProvider)
{
    private readonly NeuralHub _neuralHub = new NeuralHub();
    private readonly HashSet<Func<Task<IModel>>> _modelProviders = new HashSet<Func<Task<IModel>>>();
    
    public NeuralHubBuilder AddModel<TModel>(string modelPath) 
        where TModel : class, IModel
    {
        _modelProviders.Add(async () => await _modelProvider.InitializeAsync<TModel>(_fileSystemProvider, modelPath));
        
        return this;
    }

    public NeuralHubBuilder AddModel<TModel, TOptions>(TOptions options) 
        where TModel : class, IModel<TOptions> 
        where TOptions : class, IModelOptions
    {
        _modelProviders.Add(() => _modelProvider.InitializeAsync<TModel, TOptions>(_fileSystemProvider, options));
        
        return this;
    }
    
    public async Task<NeuralHub> BuildAsync()
    {
        await Parallel.ForEachAsync(_modelProviders, async (modelProvider, _) =>
        {
            var model = await modelProvider();
            _neuralHub.Models.Add(model);
        });
        
        return _neuralHub;
    }
}
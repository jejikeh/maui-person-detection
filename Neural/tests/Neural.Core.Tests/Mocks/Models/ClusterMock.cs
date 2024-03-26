using Neural.Core.Models;

namespace Neural.Core.Tests.Mocks.Models;

public class ClusterMock<TModel, TModelTask> : ICluster<TModel, TModelTask> 
    where TModel : class, IModel<TModelTask> 
    where TModelTask : class, IModelTask
{
    private readonly List<TModel> _models = [];
    
    public bool Init(NeuralHub neuralHub)
    {
        var models = neuralHub.GetModels<TModel>().ToArray();
        
        if (models.Length == 0)
        {
            return false;
        }
        
        _models.AddRange(models);
        
        return true;
    }

    public TModel? GetModelWithStatus(ModelStatus status)
    {
        return _models.FirstOrDefault(model => model.Status == status);
    }

    public async Task RunHandleAsync(TModelTask input, Func<TModelTask, Task> handleModelCompleted)
    {
        var output = await RunAsync(input);

        if (output is not null)
        {
            await handleModelCompleted(output);
        }
    }

    public async Task RunHandleAsync(IEnumerable<TModelTask> inputs, Func<TModelTask, Task> handleModelCompleted)
    {
        foreach (var input in inputs)
        {
            await handleModelCompleted(await RunAsync(input) ?? throw new InvalidOperationException());
        }
    }

    public Task<TModelTask?> RunAsync(TModelTask input)
    {
        var model = GetModelWithStatus(ModelStatus.Inactive);

        if (model is null)
        {
            return Task.FromResult<TModelTask?>(null);
        }
        
        return model.RunAsync(input)!;
    }

    public int Count()
    {
        return _models.Count;
    }

    public Task<TModelTask?> RunInBackgroundAsync(TModelTask input)
    {
        var model = GetModelWithStatus(ModelStatus.Inactive);

        if (model is null)
        {
            return Task.FromResult<TModelTask?>(null);
        }
        
        var modelTask = model.TryRunInBackground(input);
        
        return Task.FromResult(modelTask)!;
    }
}
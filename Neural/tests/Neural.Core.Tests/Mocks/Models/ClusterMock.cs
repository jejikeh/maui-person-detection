using Neural.Core.Models;

namespace Neural.Core.Tests.Mocks.Models;

public class ClusterMock<TModel, TModelTask> : ICluster<TModel, TModelTask> 
    where TModel : class, IModel<TModelTask> 
    where TModelTask : class, IModelTask
{
    private readonly List<TModel> _models = [];

    public void AddRange(IEnumerable<TModel> models)
    {
        _models.AddRange(models);
    }

    public TModel? GetModelWithStatus(ModelStatus status)
    {
        return _models.FirstOrDefault(model => model.Status == status);
    }

    public async Task RunHandleAsync(TModelTask input, Action<TModelTask> handleModelCompleted)
    {
        var output = await RunAsync(input);

        if (output is not null)
        {
            handleModelCompleted(output);
        }
    }

    public async Task RunHandleAsync(IEnumerable<TModelTask> inputs, Action<TModelTask> handleModelCompleted)
    {
        foreach (var input in inputs)
        {
            handleModelCompleted(await RunAsync(input) ?? throw new InvalidOperationException());
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
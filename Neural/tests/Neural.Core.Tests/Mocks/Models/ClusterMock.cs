using Neural.Core.Models;

namespace Neural.Core.Tests.Mocks.Models;

public class ClusterMock<TModel, TModelTask> : ICluster<TModel, TModelTask> 
    where TModel : IModel<TModelTask> 
    where TModelTask : class, IModelTask
{
    private readonly List<TModel> _models = [];
    
    public event Action<TModelTask>? OnModelTaskCompleted;
    
    public void AddRange(IEnumerable<TModel> models)
    {
        _models.AddRange(models);
    }

    public TModel? GetModelWithStatus(ModelStatus status)
    {
        return _models.FirstOrDefault(model => model.Status == status);
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

    public TModelTask? RunInBackground(TModelTask input)
    {
        var model = GetModelWithStatus(ModelStatus.Inactive);

        if (model is null)
        {
            return null;
        }
        
        return model.TryRunInBackground(input)!;
    }
}
using System.Collections.Concurrent;
using Neural.Core.Models;

namespace Neural.Defaults.Models;

public class Cluster<TModel, TModelTask> : ICluster<TModel, TModelTask> 
    where TModel : IModel<TModelTask> 
    where TModelTask : class, IModelTask
{
    private readonly List<TModel> _models = [];
    private readonly ConcurrentStack<TModel> _runningModels = new ConcurrentStack<TModel>();
    
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
        
        var modelTask = model.TryRunInBackground(input);
        
        _runningModels.Push(model);

        modelTask.OnModelTaskCompleted += (node, task) =>
        {
            _runningModels.TryPop(out _);

            if (_runningModels.IsEmpty)
            {
                OnModelTaskCompleted?.Invoke(task as TModelTask ?? throw new InvalidOperationException());
            }
        };
        
        return modelTask;
    }
}
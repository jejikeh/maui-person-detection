namespace Neural.Core.Models;

public interface ICluster<TModel, TModelTask> 
    where TModel : IModel<TModelTask> 
    where TModelTask : IModelTask
{
    public event Action<TModelTask>? OnModelTaskCompleted;

    public TModel? GetModelWithStatus(ModelStatus status);
    
    public bool IsAnyModelWithStatus(ModelStatus status)
    {
        return GetModelWithStatus(status) is not null;
    }

    public Task<TModelTask?> RunAsync(TModelTask input);
    TModelTask? RunInBackground(TModelTask input);
    
    public void AddRange(IEnumerable<TModel> models);
    public int Count();
}
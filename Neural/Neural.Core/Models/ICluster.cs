using System.Threading.Tasks.Dataflow;

namespace Neural.Core.Models;

public interface ICluster<TModel, TModelTask> 
    where TModel : IModel<TModelTask> 
    where TModelTask : IModelTask
{
    public TModel? GetModelWithStatus(ModelStatus status);

    public Task RunHandleAsync(IEnumerable<TModelTask> inputs, Action<TModelTask> handleModelCompleted);
    public Task<TModelTask?> RunAsync(TModelTask input);
    public Task<TModelTask?> RunInBackgroundAsync(TModelTask input);
    
    public void AddRange(IEnumerable<TModel> models);
    public int Count();
    
    public bool IsAnyModelWithStatus(ModelStatus status)
    {
        return GetModelWithStatus(status) is not null;
    }
}
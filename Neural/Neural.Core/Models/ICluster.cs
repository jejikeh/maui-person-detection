namespace Neural.Core.Models;

public interface ICluster : IInitFromNeuralHub;

public interface ICluster<out TModel, TModelTask> : ICluster
    where TModel : IModel<TModelTask> 
    where TModelTask : IModelTask
{
    public TModel? GetModelWithStatus(ModelStatus status);
    public Task RunHandleAsync(TModelTask input, Func<TModelTask, Task> handleModelCompleted);
    public Task RunHandleAsync(IEnumerable<TModelTask> inputs, Func<TModelTask, Task> handleModelCompleted);
    public Task<TModelTask?> RunAsync(TModelTask input);
    public Task<TModelTask?> RunInBackgroundAsync(TModelTask input);
    public int Count();
}
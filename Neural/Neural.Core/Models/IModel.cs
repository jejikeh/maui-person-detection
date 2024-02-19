namespace Neural.Core.Models;

public interface IModel
{
    public string Name { get; set; }
    public ModelStatus Status { get; set; }
    public IModelWorker? Worker { get; set; }
    public bool CanProcess(IModelTask modelTask);

    public void Initialize(IModelWorker modelWorker)
    {
        Worker = modelWorker;
    }
}

public interface IModel<TModelTask> : IModel
    where TModelTask : IModelTask
{
    public Task<TModelTask> RunAsync(TModelTask input);
    public TModelTask TryRunInBackground(TModelTask input);

    bool IModel.CanProcess(IModelTask modelTask)
    {
        return modelTask is TModelTask && Status == ModelStatus.Inactive;
    }
}

public interface IModel<TModelTask, TOptions> : IModel<TModelTask> 
    where TOptions : IModelOptions 
    where TModelTask : IModelTask
{
    public TOptions? Options { get; set; }
    
    public void Initialize(IModelWorker modelWorker, TOptions options)
    {
        Initialize(modelWorker);
        Options = options;
    }
}
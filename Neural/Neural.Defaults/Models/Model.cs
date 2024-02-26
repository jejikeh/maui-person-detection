using Neural.Core.Models;
using Neural.Core.Models.Events;

namespace Neural.Defaults.Models;

public abstract class Model<TModelTask> : IModel<TModelTask> 
    where TModelTask : IModelTask 
{
    private ModelStatus _status = ModelStatus.Inactive;

    public ModelStatus Status
    {
        get => _status;
        set
        {
            _status = value;
            StatusChanged?.Invoke(this, new ModelStatusChangedEventArgs(value));
        }
    }

    public event EventHandler<ModelStatusChangedEventArgs>? StatusChanged;
    public string Name { get; set; } = nameof(Model<TModelTask>);

    public async Task<TModelTask> RunAsync(TModelTask task)
    {
        Status = ModelStatus.Active;
        
        var result = await ProcessAsync(task);
        
        Status = ModelStatus.Inactive;
        
        return result;
    }
    
    public TModelTask TryRunInBackground(TModelTask input)
    {
        Status = ModelStatus.Active;

        Task.Run(async () => await RunAsync(input));

        return input;
    }
    
    public void Initialize()
    {
    }

    protected abstract Task<TModelTask> ProcessAsync(TModelTask task);
}

public abstract class Model<TModelTask, TDependencyContainer> : IModel<TModelTask, TDependencyContainer> 
    where TModelTask : IModelTask 
    where TDependencyContainer : class, IDependencyContainer
{
    private ModelStatus _status = ModelStatus.Inactive;

    public ModelStatus Status
    {
        get => _status;
        set
        {
            _status = value;
            StatusChanged?.Invoke(this, new ModelStatusChangedEventArgs(value));
        }
    }
    
    public TDependencyContainer? DependencyContainer { get; set; }
    public event EventHandler<ModelStatusChangedEventArgs>? StatusChanged;
    public string Name { get; set; } = Guid.NewGuid().ToString();

    public async Task<TModelTask> RunAsync(TModelTask task)
    {
        if (DependencyContainer is null)
        {
            throw new NullReferenceException(nameof(DependencyContainer));
        }
        
        Status = ModelStatus.Active;
        
        var result = await ProcessAsync(task);
        
        Status = ModelStatus.Inactive;
        
        return result;
    }
    
    public TModelTask TryRunInBackground(TModelTask input)
    {
        Status = ModelStatus.Active;

        Task.Run(async () => await RunAsync(input));

        return input;
    }

    protected abstract Task<TModelTask> ProcessAsync(TModelTask task);
}
using Neural.Core.Common;
using Neural.Core.Models.Events;

namespace Neural.Core.Models;

public interface IModel
{
    public string Name { get; set; }
    public ModelStatus Status { get; set; }
    
    public bool CanProcess(IModelTask modelTask);
    public void Initialize();
    
    public event EventHandler<ModelStatusChangedEventArgs> StatusChanged; 
}

public interface IModel<TModelTask> : IModel
    where TModelTask : IModelTask
{
    public Task<TModelTask> RunAsync(TModelTask task);
    public TModelTask TryRunInBackground(TModelTask input);

    bool IModel.CanProcess(IModelTask modelTask)
    {
        return modelTask is TModelTask && Status == ModelStatus.Inactive;
    }
}

public interface IModel<TModelTask, TDependencyContainer> : IModel<TModelTask> 
    where TDependencyContainer : class, IDependencyContainer 
    where TModelTask : IModelTask
{
    public TDependencyContainer? DependencyContainer { get; set; }

    public void Initialize(TDependencyContainer dependencyContainer)
    {
        DependencyContainer = dependencyContainer;
    }

    void IModel.Initialize()
    {
        throw new InvalidModelInitializationException($"Model {Name} cannot be initialized directly. (Without dependency container)");
    }
}
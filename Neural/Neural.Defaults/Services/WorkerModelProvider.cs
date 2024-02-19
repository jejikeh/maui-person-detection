using Neural.Core.Models;
using Neural.Core.Services;

namespace Neural.Defaults.Services;

public class WorkerModelProvider : IModelProvider
{
    public IModel Initialize<TModel, TModelTask>() 
        where TModel : class, IModel<TModelTask> 
        where TModelTask : IModelTask
    {
        var model = InitializeModelInstance<TModel, TModelTask>();
        
        return model as IModel ?? throw new InvalidOperationException();
    }

    public IModel Initialize<TModel, TModelTask, TDependencyContainer>(TDependencyContainer dependencyContainer) 
        where TModel : class, IModel<TModelTask, TDependencyContainer> 
        where TModelTask : IModelTask 
        where TDependencyContainer : class, IDependencyContainer
    {
        var model = InitializeModelInstanceWithDependency<TModel, TModelTask, TDependencyContainer>(dependencyContainer);
        
        return model as IModel ?? throw new InvalidOperationException();
    }
    
    private static TModel InitializeModelInstance<TModel, TModelTask>()
        where TModel : class, IModel<TModelTask> 
        where TModelTask : IModelTask
    {
        var model = Activator.CreateInstance<TModel>();
        model.Initialize();
        
        return model ?? throw new InvalidOperationException();
    }

    private static TModel InitializeModelInstanceWithDependency<TModel, TModelTask, TDependencyContainer>(TDependencyContainer dependencyContainer)
        where TModel : class, IModel<TModelTask, TDependencyContainer>
        where TModelTask : IModelTask
        where TDependencyContainer : class, IDependencyContainer
    {
        var model = Activator.CreateInstance<TModel>();
        model.Initialize(dependencyContainer);
        
        return model ?? throw new InvalidOperationException();
    }
}
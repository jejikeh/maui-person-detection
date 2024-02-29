using Neural.Core.Models;

namespace Neural.Core.Services;

public interface IModelProvider
{
    public IModel Initialize<TModel, TModelTask>() 
        where TModel : class, IModel<TModelTask>
        where TModelTask : IModelTask;

    public IModel Initialize<TModel, TModelTask, TDependencyContainer>(TDependencyContainer dependencyContainer)
        where TModel : class, IModel<TModelTask, TDependencyContainer>
        where TModelTask : IModelTask
        where TDependencyContainer : class, IDependencyContainer;
}
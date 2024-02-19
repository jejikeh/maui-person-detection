using Neural.Core.Models;

namespace Neural.Core.Services;

public interface IModelProvider
{
    public Task<IModel> InitializeAsync<TModel, TModelTask, TWorkerOptions>(IDependencyProvider dependencyProvider, TWorkerOptions options) 
        where TModel : class, IModel<TModelTask>
        where TModelTask : IModelTask;

    public Task<IModel> InitializeAsync<TModel, TModelTask, TOptions, TWorkerOptions>(IDependencyProvider dependencyProvider, TOptions modelOptions, TWorkerOptions workerOptions)
        where TModel : class, IModel<TModelTask, TOptions>
        where TOptions : IModelOptions
        where TModelTask : IModelTask;
}
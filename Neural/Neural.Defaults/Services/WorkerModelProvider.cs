using Neural.Core.Models;
using Neural.Core.Services;

namespace Neural.Defaults.Services;

public class WorkerModelProvider : IModelProvider
{
    public async Task<IModel> InitializeAsync<TModel, TModelTask, TWorkerOptions>(IDependencyProvider dependencyProvider, TWorkerOptions modelOptions)
        where TModel : class, IModel<TModelTask> 
        where TModelTask : IModelTask
    {
        var model = InitializeModelInstance<TModel, TModelTask>(await dependencyProvider.CreateContainerAsync(modelOptions));
        
        return model as IModel ?? throw new InvalidOperationException();
    }
    
    public async Task<IModel> InitializeAsync<TModel, TModelTask, TOptions, TWorkerOptions>(IDependencyProvider dependencyProvider, TOptions modelOptions, TWorkerOptions modelWorkerOptions) 
        where TModel : class, IModel<TModelTask, TOptions>
        where TOptions : IModelOptions
        where TModelTask : IModelTask
    {
        var model = InitializeModelInstance<TModel, TModelTask, TOptions>(
            await dependencyProvider.CreateContainerAsync(modelWorkerOptions), 
            modelOptions);
        
        return model as IModel ?? throw new InvalidOperationException();
    }
    
    private static TModel InitializeModelInstance<TModel, TModelTask>(IDependencyContainer dependencyContainer)
        where TModel : class, IModel<TModelTask> 
        where TModelTask : IModelTask
    {
        var model = Activator.CreateInstance<TModel>();
        model.Initialize(dependencyContainer);
        
        return model ?? throw new InvalidOperationException();
    }

    private static TModel InitializeModelInstance<TModel, TModelTask, TOptions>(IDependencyContainer dependencyContainer, TOptions modelOptions)
        where TModel : class, IModel<TModelTask, TOptions>
        where TOptions : IModelOptions
        where TModelTask : IModelTask
    {
        var model = Activator.CreateInstance<TModel>();
        model.Initialize(dependencyContainer, modelOptions);
        
        return model ?? throw new InvalidOperationException();
    }
}
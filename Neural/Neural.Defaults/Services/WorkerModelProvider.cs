using Neural.Core.Models;
using Neural.Core.Services;

namespace Neural.Defaults.Services;

public class WorkerModelProvider : IModelProvider
{
    public async Task<IModel> InitializeAsync<TModel, TModelTask, TWorkerOptions>(IModelWorkerProvider modelWorkerProvider, TWorkerOptions modelOptions)
        where TModel : class, IModel<TModelTask> 
        where TModelTask : IModelTask
    {
        var model = InitializeModelInstance<TModel, TModelTask>(await modelWorkerProvider.CreateWorkerAsync(modelOptions));
        
        return model as IModel ?? throw new InvalidOperationException();
    }
    
    public async Task<IModel> InitializeAsync<TModel, TModelTask, TOptions, TWorkerOptions>(IModelWorkerProvider modelWorkerProvider, TOptions modelOptions, TWorkerOptions modelWorkerOptions) 
        where TModel : class, IModel<TModelTask, TOptions>
        where TOptions : IModelOptions
        where TModelTask : IModelTask
    {
        var model = InitializeModelInstance<TModel, TModelTask, TOptions>(
            await modelWorkerProvider.CreateWorkerAsync(modelWorkerOptions), 
            modelOptions);
        
        return model as IModel ?? throw new InvalidOperationException();
    }
    
    private static TModel InitializeModelInstance<TModel, TModelTask>(IModelWorker modelWorker)
        where TModel : class, IModel<TModelTask> 
        where TModelTask : IModelTask
    {
        var model = Activator.CreateInstance<TModel>();
        model.Initialize(modelWorker);
        
        return model ?? throw new InvalidOperationException();
    }

    private static TModel InitializeModelInstance<TModel, TModelTask, TOptions>(IModelWorker modelWorker, TOptions modelOptions)
        where TModel : class, IModel<TModelTask, TOptions>
        where TOptions : IModelOptions
        where TModelTask : IModelTask
    {
        var model = Activator.CreateInstance<TModel>();
        model.Initialize(modelWorker, modelOptions);
        
        return model ?? throw new InvalidOperationException();
    }
}
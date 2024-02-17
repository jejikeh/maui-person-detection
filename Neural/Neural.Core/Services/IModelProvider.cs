using Neural.Core.Models;

namespace Neural.Core.Services;

public interface IModelProvider
{
    public Task<IModel> InitializeAsync<TModel, TModelTask>(IFileSystemProvider fileSystemProvider, string modelPath) 
        where TModel : class, IModel<TModelTask>
        where TModelTask : IModelTask;

    public Task<IModel> InitializeAsync<TModel, TModelTask, TOptions>(IFileSystemProvider fileSystemProvider,
        TOptions modelOptions)
        where TModel : class, IModel<TModelTask, TOptions>
        where TOptions : IModelOptions
        where TModelTask : IModelTask;
}
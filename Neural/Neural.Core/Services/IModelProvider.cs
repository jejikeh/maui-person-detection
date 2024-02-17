using Neural.Core.Models;

namespace Neural.Core.Services;

public interface IModelProvider
{
    public Task<IModel> InitializeAsync<TModel>(IFileSystemProvider fileSystemProvider, string modelPath) 
        where TModel : class, IModel;

    public Task<IModel> InitializeAsync<TModel, TOptions>(IFileSystemProvider fileSystemProvider,
        TOptions modelOptions)
        where TModel : class, IModel<TOptions>
        where TOptions : IModelOptions;
}
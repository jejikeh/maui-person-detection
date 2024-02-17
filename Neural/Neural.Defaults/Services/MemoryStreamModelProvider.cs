using Neural.Core.Models;
using Neural.Core.Services;

namespace Neural.Defaults.Services;

public class MemoryStreamModelProvider : IModelProvider
{
    public async Task<IModel> InitializeAsync<TModel, TModelTask>(IFileSystemProvider fileSystemProvider, string modelPath)
        where TModel : class, IModel<TModelTask> 
        where TModelTask : IModelTask
    {
        using var modelStream = new MemoryStream();
        
        var stream = await fileSystemProvider.GetFileStreamAsync(modelPath);
        
        await stream.CopyToAsync(modelStream);
        
        var model = ModelFromStream<TModel, TModelTask>(modelStream);
        
        return model as IModel ?? throw new InvalidOperationException();
    }
    
    public async Task<IModel> InitializeAsync<TModel, TModelTask, TOptions>(IFileSystemProvider fileSystemProvider, TOptions modelOptions) 
        where TModel : class, IModel<TModelTask, TOptions>
        where TOptions : IModelOptions
        where TModelTask : IModelTask
    {
        using var modelStream = new MemoryStream();

        var stream = await fileSystemProvider.GetFileStreamAsync(modelOptions.Path);

        await stream.CopyToAsync(modelStream);
        
        var model = ModelFromStream<TModel, TModelTask, TOptions>(modelStream, modelOptions);
        
        return model as IModel ?? throw new InvalidOperationException();
    }
    
    private static TModel ModelFromStream<TModel, TModelTask>(MemoryStream modelStream)
        where TModel : class, IModel<TModelTask> 
        where TModelTask : IModelTask
    {
        var model = Activator.CreateInstance<TModel>();
        model.Initialize(modelStream);
        
        return model ?? throw new InvalidOperationException();
    }

    private static TModel ModelFromStream<TModel, TModelTask, TOptions>(MemoryStream modelStream, TOptions modelOptions)
        where TModel : class, IModel<TModelTask, TOptions>
        where TOptions : IModelOptions
        where TModelTask : IModelTask
    {
        var model = Activator.CreateInstance<TModel>();
        model.Initialize(modelStream, modelOptions);
        
        return model ?? throw new InvalidOperationException();
    }
}
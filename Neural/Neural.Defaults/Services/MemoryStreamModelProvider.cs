using Neural.Core.Models;
using Neural.Core.Services;

namespace Neural.Defaults.Services;

public class MemoryStreamModelProvider : IModelProvider
{
    public async Task<IModel> InitializeAsync<TModel>(IFileSystemProvider fileSystemProvider, string modelPath)
        where TModel : class, IModel
    {
        using var modelStream = new MemoryStream();
        
        var stream = await fileSystemProvider.GetFileStreamAsync(modelPath);
        
        await stream.CopyToAsync(modelStream);
        
        var model = ModelFromStream<TModel>(modelStream);
        
        return model;
    }
    
    public async Task<IModel> InitializeAsync<TModel, TOptions>(IFileSystemProvider fileSystemProvider, TOptions modelOptions) 
        where TModel : class, IModel<TOptions>
        where TOptions : IModelOptions
    {
        using var modelStream = new MemoryStream();

        var stream = await fileSystemProvider.GetFileStreamAsync(modelOptions.Path);

        await stream.CopyToAsync(modelStream);
        
        var model = ModelFromStream<TModel, TOptions>(modelStream, modelOptions);

        return model;
    }
    
    private static TModel ModelFromStream<TModel>(MemoryStream modelStream)
        where TModel : class, IModel
    {
        var model = Activator.CreateInstance<TModel>();
        model.Initialize(modelStream);
        
        return model;
    }

    private static TModel ModelFromStream<TModel, TOptions>(MemoryStream modelStream, TOptions modelOptions)
        where TModel : class, IModel<TOptions>
        where TOptions : IModelOptions
    {
        var modelWithOptions = Activator.CreateInstance<TModel>();
        modelWithOptions.Initialize(modelStream, modelOptions);
        
        return modelWithOptions ?? throw new InvalidOperationException();
    }
}
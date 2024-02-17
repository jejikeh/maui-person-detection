using Microsoft.ML.OnnxRuntime;

namespace Neural.Core.Models;

public interface IModel
{
    public InferenceSession? InferenceSession { get; set; }
    
    public void Initialize(MemoryStream memoryStream)
    {
        InferenceSession = new InferenceSession(memoryStream.ToArray(), new SessionOptions());
    }
}

public interface IModel<TModelTask> : IModel 
    where TModelTask : IModelTask
{
    public Task<TModelTask> RunAsync(TModelTask input);
}

public interface IModel<TModelTask, TOptions> : IModel<TModelTask> 
    where TOptions : IModelOptions 
    where TModelTask : IModelTask
{
    public TOptions? Options { get; set; }
    
    public void Initialize(MemoryStream memoryStream, TOptions options)
    {
        Initialize(memoryStream);
        Options = options;
    }
}
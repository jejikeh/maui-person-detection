using Microsoft.ML.OnnxRuntime;

namespace Neural.Core.Models;

public interface IModel
{
    public ModelStatus Status { get; set; }
    public InferenceSession? InferenceSession { get; set; }
    
    public bool CanProcess(IModelTask modelTask);
    
    public void Initialize(MemoryStream memoryStream)
    {
        InferenceSession = new InferenceSession(memoryStream.ToArray(), new SessionOptions());
    }
}

public interface IModel<TModelTask> : IModel
    where TModelTask : IModelTask
{
    public Task<TModelTask> RunAsync(TModelTask input);
    
    public TModelTask TryRunInBackground(TModelTask input);

    bool IModel.CanProcess(IModelTask modelTask)
    {
        return modelTask is TModelTask && Status == ModelStatus.Inactive;
    }
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
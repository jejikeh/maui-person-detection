using Neural.Core.Models;

namespace Neural.Defaults.Models;

public abstract class ModelTask<TInput, TOutput> : IModelTask
{
    public abstract IModelInput Input { get; set; }
    
    public abstract IModelOutput Output { get; set; }
    
    public event Action<ModelTask<TInput, TOutput>>? OnModelTaskComplete;
    
    public TInput TypedInput => (TInput)Input;
    
    public TOutput TypedOutput => (TOutput)Output;
    
    public void SetOutput(IModel model, object value)
    {
        Output.Set(value);
        
        OnModelTaskComplete?.Invoke(this);
    }
}
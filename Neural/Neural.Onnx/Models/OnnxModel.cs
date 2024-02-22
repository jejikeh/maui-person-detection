using Microsoft.ML.OnnxRuntime;
using Neural.Core.Models;
using Neural.Onnx.Common.Dependencies;

namespace Neural.Onnx.Models;

public abstract class OnnxModel<TModelTask> : IModel<TModelTask, OnnxDependencies> 
    where TModelTask : IModelTask
{
    private ModelStatus _status = ModelStatus.Inactive;

    public ModelStatus Status
    {
        get => _status;
        set
        {
            _status = value;
            StatusChanged?.Invoke(this, new ModelStatusChangedEventArgs(value));
        }
    }
    
    public string Name { get; set; } = string.Empty;
    
    public OnnxDependencies? DependencyContainer { get; set; }
    
    public event EventHandler<ModelStatusChangedEventArgs>? StatusChanged;
    public InferenceSession? InferenceSession { get; set; }

    public abstract Task<TModelTask> RunAsync(TModelTask task);
    
    void IModel<TModelTask, OnnxDependencies>.Initialize(OnnxDependencies onnxDependencies)
    {
        DependencyContainer = onnxDependencies;
        
        InferenceSession = DependencyContainer.InferenceSession;
    }

    public TModelTask TryRunInBackground(TModelTask input)
    {
        Status = ModelStatus.Active;

        Task.Run(async () => await RunAsync(input));

        return input;
    }
}
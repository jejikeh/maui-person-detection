using Neural.Core.Models;
using Neural.Onnx.Common.Dependencies;

namespace Neural.Tests.Common.Mocks.Models.Yolo8;

public class Yolo8Model<TModelTask> : IModel<TModelTask, OnnxDependencies> 
    where TModelTask : IModelTask
{
    public const string MockedOutput = "I`m a mocked output";
    public const string MockedBackgroundOutput = "I`m a mocked background output";
    public const int BackgroundDelayMs = 100;
    
    public string Name { get; set; } = "Yolo8";
    public event EventHandler<ModelStatusChangedEventArgs>? StatusChanged;
    
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
    
    public OnnxDependencies? DependencyContainer { get; set; }
    
    public Task<TModelTask> RunAsync(TModelTask task)
    {
        task.Output.Set(MockedOutput);
        
        return Task.FromResult(task);
    }

    public TModelTask TryRunInBackground(TModelTask input)
    {
        Status = ModelStatus.Active;
        
        Task.Run(async () =>
        {
            await Task.Delay(BackgroundDelayMs);
            
            input.Output.Set(MockedBackgroundOutput);
            
            Status = ModelStatus.Inactive;
        });
        
        return input;
    }
}
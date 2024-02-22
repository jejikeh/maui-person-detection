using Neural.Core.Models;
using Neural.Samples.HelloSumOfNumberPipeline.Common.Dependencies;
using Neural.Samples.HelloSumOfNumberPipeline.Services;
using Neural.Samples.HelloSumOfNumberPipeline.Tasks.IntToString;

namespace Neural.Samples.HelloSumOfNumberPipeline.Models;

public class HelloNumberModel : IModel<IntToStringTask, HelloWorldDependencies>
{
    public HelloWorldDependencies? DependencyContainer { get; set; }
    public string Name { get; set; } = string.Empty;
    
    public ModelStatus Status
    {
        get => _status;
        set
        {
            _status = value;
            StatusChanged?.Invoke(this, new ModelStatusChangedEventArgs(value));
        }
    }
    
    public event EventHandler<ModelStatusChangedEventArgs>? StatusChanged;

    private HelloNumberService? _helloNumberService;
    private ModelStatus _status = ModelStatus.Inactive;

    void IModel<IntToStringTask, HelloWorldDependencies>.Initialize(HelloWorldDependencies dependencyContainer)
    {
        DependencyContainer = dependencyContainer;
        
        _helloNumberService = dependencyContainer.HelloNumberService;
    }

    public Task<IntToStringTask> RunAsync(IntToStringTask task)
    {
        if (DependencyContainer is null)
        {
            throw new NullReferenceException(nameof(DependencyContainer));
        }
        
        Status = ModelStatus.Active;

        _helloNumberService?.Hello(task.InputInput().Value ?? 0);
        
        Status = ModelStatus.Inactive;
        
        return Task.FromResult(task);
    }
    
    public IntToStringTask TryRunInBackground(IntToStringTask input)
    {
        Status = ModelStatus.Active;

        Task.Run(async () => await RunAsync(input));

        return input;
    }
}
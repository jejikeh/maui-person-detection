using Neural.Core.Models;
using Neural.Tests.Common.Mocks.Samples.HelloNumberCluster.Common.Dependencies;
using Neural.Tests.Common.Mocks.Samples.HelloNumberCluster.Tasks.IntToString;

namespace Neural.Tests.Common.Mocks.Samples.HelloNumberCluster.Models;

public class HelloNumberModel : IModel<IntToStringTask, HelloNumberDependencies>
{
    public string Name { get; set; } = "HelloNumber";
    public HelloNumberDependencies? DependencyContainer { get; set; }
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
    
    public Task<IntToStringTask> RunAsync(IntToStringTask task)
    {
        if (DependencyContainer is null)
        {
            throw new NullReferenceException(nameof(DependencyContainer));
        }
        
        Status = ModelStatus.Active;
        
        var sum = DependencyContainer.HelloNumberService.SayHello(task.IntInput().Value);
        
        task.SetOutput(this, sum);
        
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
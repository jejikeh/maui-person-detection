using Neural.Core.Models;
using Neural.Core.Models.Events;
using Neural.Samples.BackgroundHelloWorld.Common;
using Neural.Samples.BackgroundHelloWorld.Common.Dependencies;
using Neural.Samples.BackgroundHelloWorld.Tasks.StringToString;

namespace Neural.Samples.BackgroundHelloWorld.Models;

public class HelloWorldModel : IModel<StringToStringTask, HelloWorldDependencies>
{
    public event EventHandler<ModelStatusChangedEventArgs>? StatusChanged;
    public string Name { get; set; } = string.Empty;
    public HelloWorldDependencies? DependencyContainer { get; set; }
    
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

    void IModel<StringToStringTask, HelloWorldDependencies>.Initialize(HelloWorldDependencies dependencyContainer)
    {
        DependencyContainer = dependencyContainer;
        Name = dependencyContainer.ModelNameProvider.GetModelName();
    }

    public async Task<StringToStringTask> RunAsync(StringToStringTask task)
    {
        if (DependencyContainer is null)
        {
            throw new NullReferenceException(nameof(DependencyContainer));
        }
        
        Status = ModelStatus.Active;
        
        if (task.StringInput().Value!.Equals(Constants.HelloMessage))
        {
            task.SetOutput(this, await DependencyContainer.HelloWorldService.HelloAsync());
        }
        
        if (task.StringInput().Value!.Equals(Constants.ByeMessage))
        {
            task.SetOutput(this, await DependencyContainer.HelloWorldService.ByeAsync());
        }
        
        Status = ModelStatus.Inactive;
        
        return task;
    }
    
    public StringToStringTask TryRunInBackground(StringToStringTask input)
    {
        Status = ModelStatus.Active;

        Task.Run(async () => await RunAsync(input));

        return input;
    }
}
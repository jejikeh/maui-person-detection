using Neural.BackgroundHelloWorld.Common;
using Neural.BackgroundHelloWorld.Common.Dependencies;
using Neural.BackgroundHelloWorld.Tasks.StringToString;
using Neural.Core.Models;

namespace Neural.BackgroundHelloWorld.Models;

public class HelloWorldModel : IModel<StringToStringTask, HelloWorldDependencies>
{
    public ModelStatus Status { get; set; }
    public string Name { get; set; } = string.Empty;
    public HelloWorldDependencies? DependencyContainer { get; set; }

    void IModel<StringToStringTask, HelloWorldDependencies>.Initialize(HelloWorldDependencies dependencyContainer)
    {
        DependencyContainer = dependencyContainer;
        Name = dependencyContainer.ModelNameProvider.GetModelName();
    }

    public async Task<StringToStringTask> RunAsync(StringToStringTask input)
    {
        if (DependencyContainer is null)
        {
            throw new NullReferenceException(nameof(DependencyContainer));
        }
        
        Status = ModelStatus.Active;
        
        if (input.StringInput().Value!.Equals(Constants.HelloMessage))
        {
            input.SetOutput(this, await DependencyContainer.HelloWorldService.HelloAsync());
        }
        
        if (input.StringInput().Value!.Equals(Constants.ByeMessage))
        {
            input.SetOutput(this, await DependencyContainer.HelloWorldService.ByeAsync());
        }
        
        Status = ModelStatus.Inactive;
        
        return input;
    }
    
    public StringToStringTask TryRunInBackground(StringToStringTask input)
    {
        Status = ModelStatus.Active;

        Task.Run(async () => await RunAsync(input));

        return input;
    }
}
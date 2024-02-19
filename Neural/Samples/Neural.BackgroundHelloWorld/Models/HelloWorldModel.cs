using Neural.BackgroundHelloWorld.Common;
using Neural.BackgroundHelloWorld.Services;
using Neural.BackgroundHelloWorld.Tasks.StringToString;
using Neural.Core.Models;

namespace Neural.BackgroundHelloWorld.Models;

public class HelloWorldModel : IModel<StringToStringTask>
{
    public string Name { get; set; } = Guid.NewGuid().ToString();
    public ModelStatus Status { get; set; }
    public IDependencyContainer? DependencyContainer { get; set; }
    
    public HelloWorldService HelloWorldService => DependencyContainer!.CastToDependency<HelloWorldService>();
    
    public async Task<StringToStringTask> RunAsync(StringToStringTask input)
    {
        Status = ModelStatus.Active;
        
        await Task.Delay(1000);
        
        if (input.StringInput().Value!.Equals(Constants.HelloMessage))
        {
            input.SetOutput(Name, HelloWorldService.Hello());
        }
        
        if (input.StringInput().Value!.Equals(Constants.ByeMessage))
        {
            input.SetOutput(Name, HelloWorldService.Bye());
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
using Neural.Core.Models;
using Neural.Defaults.Common.Dependencies;

namespace Neural.Tests.Common.Mocks.Models.Yolo8;

public class Yolo8Model<TModelTask> : IModel<TModelTask, OnnxDependencies> 
    where TModelTask : IModelTask
{
    public const string MockedOutput = "I`m a mocked output";
    public const string MockedBackgroundOutput = "I`m a mocked background output";
    
    public string Name { get; set; } = "Yolo8";
    public ModelStatus Status { get; set; } = ModelStatus.Inactive;
    public OnnxDependencies? DependencyContainer { get; set; }
    
    public Task<TModelTask> RunAsync(TModelTask input)
    {
        input.Output.Set(MockedOutput);
        
        return Task.FromResult(input);
    }

    public TModelTask TryRunInBackground(TModelTask input)
    {
        Status = ModelStatus.Active;
        
        Task.Run(async () =>
        {
            await Task.Delay(100);
            
            input.Output.Set(MockedBackgroundOutput);
            
            Status = ModelStatus.Inactive;
        });
        
        return input;
    }
}
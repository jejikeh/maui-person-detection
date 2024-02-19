using Neural.Core.Models;
using Neural.Defaults.Common.Dependencies;

namespace Neural.Tests.Common.Mocks.Models.Yolo5;

public class Yolo5Model<TModelTask> : IModel<TModelTask, OnnxDependencies> 
    where TModelTask : IModelTask
{
    public const string MockedOutput = "I`m a mocked output";
    public const string MockedBackgroundOutput = "I`m a mocked background output";
    public const int BackgroundDelayMs = 100;
    
    public string Name { get; set; } = "Yolo5";
    public ModelStatus Status { get; set; } = ModelStatus.Inactive;
    public OnnxDependencies? DependencyContainer { get; set; }
    
    public Task<TModelTask> RunAsync(TModelTask input)
    {
        input.SetOutput(this, MockedOutput);
        
        return Task.FromResult(input);
    }

    public TModelTask TryRunInBackground(TModelTask input)
    {
        Status = ModelStatus.Active;
        
        Task.Run(async () =>
        {
            await Task.Delay(BackgroundDelayMs);
            
            input.SetOutput(this, MockedBackgroundOutput);
            
            Status = ModelStatus.Inactive;
        });
        
        return input;
    }
}
using Microsoft.ML.OnnxRuntime;
using Neural.Core.Models;
using Neural.Tests.Common.Mocks.Models.Tasks;

namespace Neural.Tests.Common.Mocks.Models.Yolo8;

public class Yolo8ModelMock : IModel<IntToStringTaskMock>
{
    public const string MockedOutput = "I`m a mocked output";
    public const string MockedBackgroundOutput = "I`m a mocked background output";

    public string Name { get; set; } = Guid.NewGuid().ToString();
    public ModelStatus Status { get; set; } = ModelStatus.Inactive;
    public IDependencyContainer? DependencyContainer { get; set; }

    public Task<IntToStringTaskMock> RunAsync(IntToStringTaskMock input)
    {
        input.Output.Set(MockedOutput);
        
        return Task.FromResult(input);
    }

    public IntToStringTaskMock TryRunInBackground(IntToStringTaskMock input)
    {
        Task.Run(async () =>
        {
            await Task.Delay(100);
            
            input.Output.Set(MockedBackgroundOutput);
        });
        
        return input;
    }
}
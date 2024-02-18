using Microsoft.ML.OnnxRuntime;
using Neural.Core.Models;
using Neural.Tests.Common.Mocks.Models.Tasks;

namespace Neural.Tests.Common.Mocks.Models.Yolo5;

public class Yolo5ModelMock : IModel<StringToStringTaskMock>
{
    public const string MockedOutput = "I`m a mock";
    public const string MockedBackgroundOutput = "I`m a mocked background output";

    public string Name { get; set; } = Guid.NewGuid().ToString();
    public ModelStatus Status { get; set; } = ModelStatus.Inactive;
    public InferenceSession? InferenceSession { get; set; }
    
    public async Task<StringToStringTaskMock> RunAsync(StringToStringTaskMock input)
    {
        Status = ModelStatus.Active;
        
        await Task.Delay(100);

        input.SetOutput(Name, MockedOutput);
        
        Status = ModelStatus.Inactive;
        
        return input;
    }
    
    public StringToStringTaskMock TryRunInBackground(StringToStringTaskMock input)
    {
        Status = ModelStatus.Active;

        // In real implementation we should use RunAsync, but in mock we need to distinguish between background and foreground tasks
        // Task.Run(async () => await RunAsync(input));

        Task.Run(async () =>
        {
            await Task.Delay(100);

            input.SetOutput(Name, MockedBackgroundOutput);

            Status = ModelStatus.Inactive;
        });
        
        return input;
    }
}
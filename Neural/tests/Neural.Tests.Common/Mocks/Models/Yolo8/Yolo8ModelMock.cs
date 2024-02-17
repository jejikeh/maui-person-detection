using Microsoft.ML.OnnxRuntime;
using Neural.Core.Models;
using Neural.Tests.Common.Mocks.Models.Tasks;

namespace Neural.Tests.Common.Mocks.Models.Yolo8;

public class Yolo8ModelMock : IModel<IntToStringTaskMock>
{
    public const string MockedOutput = "I`m a mocked output";
    
    public InferenceSession? InferenceSession { get; set; }
    
    public Task<IntToStringTaskMock> RunAsync(IntToStringTaskMock input)
    {
        input.Output.Set(MockedOutput);
        
        return Task.FromResult(input);
    }
}
using Microsoft.ML.OnnxRuntime;
using Neural.Core.Models;
using Neural.Tests.Common.Mocks.Models.Tasks;

namespace Neural.Tests.Common.Mocks.Models.Yolo5;

public class Yolo5ModelIntToStringMock : IModel<IntToStringTaskMock>
{
    public const string MockedOutput = "I`m a mock";
    
    public InferenceSession? InferenceSession { get; set; }
    
    public Task<IntToStringTaskMock> RunAsync(IntToStringTaskMock input)
    {
        input.Output.Set(MockedOutput);
        
        return Task.FromResult(input);
    }
}
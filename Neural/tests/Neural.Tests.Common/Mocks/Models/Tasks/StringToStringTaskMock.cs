using Neural.Core.Models;
using Neural.Tests.Common.Mocks.Models.Tasks.Inputs;
using Neural.Tests.Common.Mocks.Models.Tasks.Outputs;

namespace Neural.Tests.Common.Mocks.Models.Tasks;

public class StringToStringTaskMock(string _input) : IModelTask
{
    public event Action<StringToStringTaskMock>? OnModelTaskCompleted;

    public IModelInput Input { get; set; } = new StringInput(_input);
    public IModelOutput Output { get; set; } = new StringOutput();
    
    public StringOutput StringOutput => (StringOutput) Output;
    
    public void SetOutput(string _, object value)
    {
        Output.Set(value);
        OnModelTaskCompleted?.Invoke(this);
    }
}
using Neural.Core.Models;
using Neural.Tests.Common.Mocks.Models.Tasks.Inputs;
using Neural.Tests.Common.Mocks.Models.Tasks.Outputs;

namespace Neural.Tests.Common.Mocks.Models.Tasks;

public class IntToStringTaskMock(int _value) : IModelTask
{
    public IModelInput Input { get; set; } = new IntInput(_value);
    public IModelOutput Output { get; set; } = new StringOutput();

    public void SetOutput(object value)
    {
        Output.Set(value);
        OnModelTaskCompleted?.Invoke(this);
    }

    public event Action<IModelTask>? OnModelTaskCompleted;
}
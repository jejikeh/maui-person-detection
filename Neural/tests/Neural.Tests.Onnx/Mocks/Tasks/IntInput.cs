using Neural.Core.Models;

namespace Neural.Tests.Onnx.Mocks.Tasks;

public class IntInput(int _value) : IModelInput
{
    public int Value { get; set; } = _value;
}
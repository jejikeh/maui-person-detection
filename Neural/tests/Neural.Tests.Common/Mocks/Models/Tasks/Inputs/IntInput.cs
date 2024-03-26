using Neural.Core.Models;

namespace Neural.Tests.Common.Mocks.Models.Tasks.Inputs;

public class IntInput(int _value) : IModelInput
{
    public int Value { get; set; } = _value;
}
using Neural.Core.Models;

namespace Neural.Tests.Common.Mocks.Models.Tasks.Inputs;

public class StringInput(string _value) : IModelInput
{
    public string Value { get; set; } = _value;
}
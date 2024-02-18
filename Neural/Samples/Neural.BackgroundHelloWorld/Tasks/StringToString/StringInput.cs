using Neural.Core.Models;

namespace Neural.BackgroundHelloWorld.Tasks.StringToString;

public class StringInput(string _value) : IModelInput
{
    public string? Value { get; set; } = _value;
}
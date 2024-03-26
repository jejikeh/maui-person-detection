using Neural.Core.Models;

namespace Neural.Samples.HelloSumOfNumberPipeline.Tasks.IntToString;

public class IntInput(int _value) : IModelInput
{
    public int? Value { get; set; } = _value;
}
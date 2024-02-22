using Neural.Core.Models;

namespace Neural.Samples.HelloSumOfNumberPipeline.Tasks.IntToInt;

public class IntsInput(int[] _value) : IModelInput
{
    public int[] Value => _value;
}
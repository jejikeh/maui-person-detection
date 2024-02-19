using Neural.Core.Models;

namespace Neural.Samples.SumOfNumbersCluster.Tasks.IntToInt;

public class IntsInput(int[] _value) : IModelInput
{
    public int[] Value => _value;
}
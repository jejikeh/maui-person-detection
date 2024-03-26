using Neural.Core.Models;

namespace Neural.Tests.Common.Mocks.Samples.SumOfNumbersCluster.Tasks.IntToInt;

public class IntsInput(int[] _value) : IModelInput
{
    public int[] Value => _value;
}
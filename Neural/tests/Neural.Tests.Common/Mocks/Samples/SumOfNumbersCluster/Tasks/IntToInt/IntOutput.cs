using Neural.Core.Models;

namespace Neural.Tests.Common.Mocks.Samples.SumOfNumbersCluster.Tasks.IntToInt;

public class IntOutput : IModelOutput
{
    public int Value { get; set; }
    
    public void Set(object value)
    {
        Value = (int)value;
    }
}
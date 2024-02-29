using Neural.Core.Models;

namespace Neural.Samples.HelloSumOfNumberPipeline.Tasks.IntToInt;

public class IntOutput : IModelOutput
{
    public int Value { get; set; }
    
    public void Set(object value)
    {
        Value = (int)value;
    }
}
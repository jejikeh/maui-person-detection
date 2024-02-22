using Neural.Core.Models;

namespace Neural.Samples.HelloSumOfNumberPipeline.Tasks.IntToString;

public class StringOutput : IModelOutput
{
    public string? Value { get; set; }
    
    public void Set(object value)
    {
        Value = (string)value;
    }
}
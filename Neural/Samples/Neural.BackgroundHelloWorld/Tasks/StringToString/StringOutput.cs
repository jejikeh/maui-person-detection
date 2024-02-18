using Neural.Core.Models;

namespace Neural.BackgroundHelloWorld.Tasks.StringToString;

public class StringOutput : IModelOutput
{
    public string? Value { get; set; }
    
    public void Set(object value)
    {
        Value = (string)value;
    }
}
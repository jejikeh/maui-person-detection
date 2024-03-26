using Neural.Core.Models;

namespace Neural.Tests.Onnx.Mocks.Tasks;

public class StringOutput : IModelOutput
{
    public string Value { get; set; } = string.Empty;
    
    public void Set(object value)
    {
        Value = (string) value;
    }
}
using Neural.Core.Models;
using Neural.Tests.Common.Mocks.Samples.SumOfNumbersCluster.Tasks.IntToInt;

namespace Neural.Tests.Common.Mocks.Samples.HelloNumberCluster.Tasks.IntToString;

public class IntToStringTask(int _input) : IModelTask
{
    public IModelInput Input { get; set; } = new IntInput(_input);
    public IModelOutput Output { get; set; } = new StringOutput();
    
    public IntInput IntInput()
    {
        return (IntInput)Input;
    }
    
    public StringOutput StringOutput()
    {
        return (StringOutput)Output;
    }
    
    public void SetOutput(IModel model, object value)
    {
        Output.Set(value);
    }

    public static IntToStringTask FromIntOutput(IntOutput output)
    {
        return new IntToStringTask(output.Value);
    }
}
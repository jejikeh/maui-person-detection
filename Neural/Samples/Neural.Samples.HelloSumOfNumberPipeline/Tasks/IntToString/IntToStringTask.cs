using Neural.Core.Models;
using Neural.Samples.HelloSumOfNumberPipeline.Tasks.IntToInt;

namespace Neural.Samples.HelloSumOfNumberPipeline.Tasks.IntToString;

public class IntToStringTask(int _input) : IModelTask
{
    public IModelInput Input { get; set; } = new IntInput(_input);
    public IModelOutput Output { get; set; } = new StringOutput();
    
    public IntInput InputInput()
    {
        return (IntInput)Input;
    }

    private StringOutput StringOutput()
    {
        return (StringOutput)Output;
    }
    
    public void SetOutput(IModel model, object value)
    {
        Output.Set(value);
    }

    public static IntToStringTask FromTask(IntsToIntTask? intOutput)
    {
        return new IntToStringTask(intOutput!.IntOutput().Value);
    }
}
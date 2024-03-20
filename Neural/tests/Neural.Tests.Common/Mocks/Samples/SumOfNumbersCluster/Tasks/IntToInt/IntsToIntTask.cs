using Neural.Core.Models;

namespace Neural.Tests.Common.Mocks.Samples.SumOfNumbersCluster.Tasks.IntToInt;

public class IntsToIntTask(int[] _input) : IModelTask
{
    public string ValueFromModelWithName { get; set; } = string.Empty;
    public IModelInput Input { get; set; } = new IntsInput(_input);
    public IModelOutput Output { get; set; } = new IntOutput();
    
    public IntsInput IntsInput()
    {
        return (IntsInput)Input;
    }
    
    public IntOutput IntOutput()
    {
        return (IntOutput)Output;
    }

    public void SetOutput(IModel model, object value)
    {
        Output.Set(value);
        
        ValueFromModelWithName = model.Name;
    }
}
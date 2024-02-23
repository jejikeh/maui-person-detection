using Neural.Core.Models;
using Neural.Core.Models.Events;

namespace Neural.Samples.HelloSumOfNumberPipeline.Tasks.IntToInt;

public class IntsToIntTask(int[] _input) : IModelTask
{
    public string ValueFromModelWithName { get; set; } = string.Empty;
    public IModelInput Input { get; set; } = new IntsInput(_input);
    public IModelOutput Output { get; set; } = new IntOutput();
    
    public event EventHandler<ModelTaskCompletedEventArgs>? OnModelTaskCompleted;
    
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
        
        OnModelTaskCompleted?.Invoke(this, new ModelTaskCompletedEventArgs());
    }
}
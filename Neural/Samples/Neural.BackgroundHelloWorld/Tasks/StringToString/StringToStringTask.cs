using Neural.Core.Models;

namespace Neural.BackgroundHelloWorld.Tasks.StringToString;

public class StringToStringTask(string _input) : IModelTask
{
    public IModelInput Input { get; set; } = new StringInput(_input);
    public IModelOutput Output { get; set; } = new StringOutput();
    
    public event Action<IModel, IModelTask>? OnModelTaskCompleted;
    public event Action<(string ModelName, StringOutput Output)>? OnModelTaskTypedComplete;

    public StringInput StringInput()
    {
        return (StringInput)Input;
    }

    private StringOutput StringOutput()
    {
        return (StringOutput)Output;
    }
    
    public void SetOutput(IModel model, object value)
    {
        Output.Set(value);
        
        OnModelTaskCompleted?.Invoke(model, this);

        OnModelTaskTypedComplete?.Invoke((model.Name, StringOutput()));
    }
}
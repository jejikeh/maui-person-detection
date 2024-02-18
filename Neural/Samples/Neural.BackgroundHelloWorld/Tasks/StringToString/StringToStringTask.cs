using Neural.Core.Models;

namespace Neural.BackgroundHelloWorld.Tasks.StringToString;

public class StringToStringTask(string _input) : IModelTask
{
    public IModelInput Input { get; set; } = new StringInput(_input);
    public IModelOutput Output { get; set; } = new StringOutput();

    public StringInput StringInput()
    {
        return (StringInput)Input;
    }

    private StringOutput StringOutput()
    {
        return (StringOutput)Output;
    }
    
    public void SetOutput(string modelName, object value)
    {
        Output.Set(value);
        OnModelOutput?.Invoke((modelName, StringOutput()));
    }
    
    public event Action<(string ModelName, StringOutput Output)>? OnModelOutput;
}
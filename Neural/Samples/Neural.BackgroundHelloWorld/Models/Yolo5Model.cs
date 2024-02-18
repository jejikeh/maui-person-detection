using Microsoft.ML.OnnxRuntime;
using Neural.BackgroundHelloWorld.Common;
using Neural.BackgroundHelloWorld.Tasks.StringToString;
using Neural.Core.Models;

namespace Neural.BackgroundHelloWorld.Models;

public class Yolo5Model : IModel<StringToStringTask>
{
    public ModelStatus Status { get; set; }
    public InferenceSession? InferenceSession { get; set; }
    
    public async Task<StringToStringTask> RunAsync(StringToStringTask input)
    {
        Status = ModelStatus.Active;
        
        await Task.Delay(1000);
        
        if (input.StringInput().Value!.Equals(Constants.HelloMessage))
        {
            input.SetOutput("Hello, World!");
        }
        
        if (input.StringInput().Value!.Equals(Constants.ByeMessage))
        {
            input.SetOutput("Bye, World!");
        }
        
        Status = ModelStatus.Inactive;
        
        return input;
    }
    
    public StringToStringTask TryRunInBackground(StringToStringTask input)
    {
        Status = ModelStatus.Active;

        Task.Run(async () => await RunAsync(input));

        return input;
    }
}
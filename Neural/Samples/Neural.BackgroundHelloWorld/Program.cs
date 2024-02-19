using Neural.BackgroundHelloWorld.Common;
using Neural.BackgroundHelloWorld.Configuration;
using Neural.BackgroundHelloWorld.Services;
using Neural.BackgroundHelloWorld.Tasks.StringToString;
using Neural.Defaults;

const int modelsCount = 10;
const int loopIterations = modelsCount / 2;

var neuralHub = NeuralHubConfiguration
    .FromDefaults(
        workerProvider: new HelloWorldWorkerProvider())
    .AddYolo5Models(modelsCount)
    .Build();

foreach (var i in Enumerable.Range(0, loopIterations))
{
    var helloOutput = neuralHub.TryRunInBackground(new StringToStringTask(Constants.HelloMessage));
    var byeOutput = neuralHub.TryRunInBackground(new StringToStringTask(Constants.ByeMessage));

    if (helloOutput is null || byeOutput is null)
    {
        Console.WriteLine($"Model {i} output is null");

        return;
    }

    helloOutput.OnModelOutput += output =>
    {
        Console.WriteLine($"Output from model {output.ModelName}: {output.Output.Value}");
    };
    
    byeOutput.OnModelOutput += output =>
    {
        Console.WriteLine($"Output from model {output.ModelName}: {output.Output.Value}");
    };
}

Console.ReadLine();
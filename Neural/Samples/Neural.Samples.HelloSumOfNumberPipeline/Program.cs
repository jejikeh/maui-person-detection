using Neural.Defaults;
using Neural.Samples.HelloSumOfNumberPipeline.Common;
using Neural.Samples.HelloSumOfNumberPipeline.Configuration;
using Neural.Samples.HelloSumOfNumberPipeline.Pipelines;

var neuralHub = NeuralHubConfiguration
    .FromDefaults()
    .AddSumNumbersModels(10)
    .AddHelloWorldModels(10)
    .Build();
    
var helloSumOfNumberPipeline = neuralHub.ExtractPipeline<HelloSumOfNumbersPipeline>();

if (helloSumOfNumberPipeline is null) 
{
    Console.WriteLine("Failed to initialize clusters");
    
    return;
}

await helloSumOfNumberPipeline.RunAsync(Utils.GenerateIntsToIntTasks(10));

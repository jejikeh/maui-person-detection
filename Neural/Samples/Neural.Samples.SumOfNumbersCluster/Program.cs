using Neural.Defaults;
using Neural.Samples.SumOfNumbersCluster.Configuration;
using Neural.Samples.SumOfNumbersCluster.Models;
using Neural.Samples.SumOfNumbersCluster.Tasks.IntToInt;

const int modelsCount = 10;
const int inputCount = 100;

var neuralHub = NeuralHubConfiguration
    .FromDefaults()
    .AddSumNumbersModels(modelsCount)
    .Build();

var sumNumbersCluster = neuralHub.ShapeCluster<SumNumbersModel, IntsToIntTask>();

var numbersInput = Enumerable.Repeat(Enumerable.Range(0, inputCount), inputCount);
var inputTasks = numbersInput.Select(x => new IntsToIntTask(x.ToArray())).ToArray();

Console.WriteLine("Running...");

await sumNumbersCluster.RunHandleAsync(inputTasks, output =>
{
    Console.WriteLine($"{output.ValueFromModelWithName} Output: {output.IntOutput().Value}");
});

Console.WriteLine("Done.");
Console.ReadLine();
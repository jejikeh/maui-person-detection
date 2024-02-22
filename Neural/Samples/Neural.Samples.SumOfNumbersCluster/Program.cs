using Neural.Defaults;
using Neural.Defaults.Models;
using Neural.Samples.SumOfNumbersCluster.Common;
using Neural.Samples.SumOfNumbersCluster.Configuration;
using Neural.Samples.SumOfNumbersCluster.Models;
using Neural.Samples.SumOfNumbersCluster.Tasks.IntToInt;

const int modelsCount = 10;
const int inputCount = 100;
var inputTasks = Utils.GenerateIntsToIntTasks(inputCount);

var neuralHub = NeuralHubConfiguration
    .FromDefaults()
    .AddSumNumbersModels(modelsCount)
    .Build();

var sumNumbersCluster = neuralHub.ShapeCluster<Cluster<SumNumbersModel, IntsToIntTask>>();

await sumNumbersCluster?.RunHandleAsync(inputTasks, output =>
{
    return Task.Run(() => Console.WriteLine($"{output.ValueFromModelWithName} Output: {output.IntOutput().Value}"));
})!;
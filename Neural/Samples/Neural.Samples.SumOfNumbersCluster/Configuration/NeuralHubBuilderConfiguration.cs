using Neural.Core;
using Neural.Samples.SumOfNumbersCluster.Common.Dependencies;
using Neural.Samples.SumOfNumbersCluster.Models;
using Neural.Samples.SumOfNumbersCluster.Tasks.IntToInt;

namespace Neural.Samples.SumOfNumbersCluster.Configuration;

public static class NeuralHubBuilderConfiguration
{
    public static NeuralHubBuilder AddSumNumbersModels(this NeuralHubBuilder neuralHub, int count)
    {
        foreach (var _ in Enumerable.Range(0, count))
        {
            neuralHub.AddModel<SumNumbersModel, IntsToIntTask, SumNumbersDependencies>(new SumNumbersDependencies());
        }
        
        return neuralHub;
    } 
}
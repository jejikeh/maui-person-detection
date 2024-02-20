using Neural.Core;
using Neural.Tests.Common.Mocks.Samples.SumOfNumbersCluster.Common.Dependencies;
using Neural.Tests.Common.Mocks.Samples.SumOfNumbersCluster.Models;
using Neural.Tests.Common.Mocks.Samples.SumOfNumbersCluster.Tasks.IntToInt;

namespace Neural.Tests.Common.Mocks.Samples.SumOfNumbersCluster.Configuration;

public static class NeuralHubBuilderConfiguration
{
    public static NeuralHubBuilder AddSumNumbersModelsMocks(this NeuralHubBuilder neuralHub, int count)
    {
        foreach (var _ in Enumerable.Range(0, count))
        {
            neuralHub.AddModel<SumNumbersModel, IntsToIntTask, SumNumbersDependencies>(new SumNumbersDependencies());
        }
        
        return neuralHub;
    } 
}
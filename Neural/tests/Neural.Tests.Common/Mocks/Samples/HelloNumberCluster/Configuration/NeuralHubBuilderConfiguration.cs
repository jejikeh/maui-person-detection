using Neural.Core;
using Neural.Tests.Common.Mocks.Samples.HelloNumberCluster.Common.Dependencies;
using Neural.Tests.Common.Mocks.Samples.HelloNumberCluster.Models;
using Neural.Tests.Common.Mocks.Samples.HelloNumberCluster.Tasks.IntToString;
using Neural.Tests.Common.Mocks.Samples.SumOfNumbersCluster.Common.Dependencies;
using Neural.Tests.Common.Mocks.Samples.SumOfNumbersCluster.Models;
using Neural.Tests.Common.Mocks.Samples.SumOfNumbersCluster.Tasks.IntToInt;

namespace Neural.Tests.Common.Mocks.Samples.HelloNumberCluster.Configuration;

public static class NeuralHubBuilderConfiguration
{
    public static NeuralHubBuilder AddHelloNumberModels(this NeuralHubBuilder neuralHub, int count)
    {
        foreach (var _ in Enumerable.Range(0, count))
        {
            neuralHub.AddModel<HelloNumberModel, IntToStringTask, HelloNumberDependencies>(new HelloNumberDependencies());
        }
        
        return neuralHub;
    } 
}
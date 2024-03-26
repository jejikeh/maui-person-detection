using Neural.Core;
using Neural.Samples.HelloSumOfNumberPipeline.Common.Dependencies;
using Neural.Samples.HelloSumOfNumberPipeline.Models;
using Neural.Samples.HelloSumOfNumberPipeline.Tasks.IntToInt;
using Neural.Samples.HelloSumOfNumberPipeline.Tasks.IntToString;

namespace Neural.Samples.HelloSumOfNumberPipeline.Configuration;

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
    
    public static NeuralHubBuilder AddHelloWorldModels(this NeuralHubBuilder neuralHub, int count)
    {
        foreach (var _ in Enumerable.Range(0, count))
        {
            neuralHub.AddModel<HelloNumberModel, IntToStringTask, HelloWorldDependencies>(new HelloWorldDependencies());
        }
        
        return neuralHub;
    } 
}
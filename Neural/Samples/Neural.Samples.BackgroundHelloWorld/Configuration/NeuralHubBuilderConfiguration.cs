using Neural.Core;
using Neural.Samples.BackgroundHelloWorld.Common.Dependencies;
using Neural.Samples.BackgroundHelloWorld.Models;
using Neural.Samples.BackgroundHelloWorld.Tasks.StringToString;

namespace Neural.Samples.BackgroundHelloWorld.Configuration;

public static class NeuralHubBuilderConfiguration
{
    public static NeuralHubBuilder AddHelloWorldModels(this NeuralHubBuilder neuralHub, int count)
    {
        foreach (var _ in Enumerable.Range(0, count))
        {
            neuralHub.AddModel<HelloWorldModel, StringToStringTask, HelloWorldDependencies>(new HelloWorldDependencies());
        }
        
        return neuralHub;
    } 
}
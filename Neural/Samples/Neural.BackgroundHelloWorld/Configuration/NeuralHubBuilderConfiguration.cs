using Neural.BackgroundHelloWorld.Common.Dependencies;
using Neural.BackgroundHelloWorld.Models;
using Neural.BackgroundHelloWorld.Tasks.StringToString;
using Neural.Core;

namespace Neural.BackgroundHelloWorld.Configuration;

public static class NeuralHubBuilderConfiguration
{
    public static NeuralHubBuilder AddYolo5Models(this NeuralHubBuilder neuralHub, int count)
    {
        foreach (var _ in Enumerable.Range(0, count))
        {
            neuralHub.AddModel<HelloWorldModel, StringToStringTask, HelloWorldDependencies>(new HelloWorldDependencies());
        }
        
        return neuralHub;
    } 
}
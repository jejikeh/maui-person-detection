using Neural.BackgroundHelloWorld.Common;
using Neural.BackgroundHelloWorld.Common.Options;
using Neural.BackgroundHelloWorld.Models;
using Neural.BackgroundHelloWorld.Tasks.StringToString;
using Neural.Core;
using Neural.Defaults.Common.Options;

namespace Neural.BackgroundHelloWorld.Configuration;

public static class NeuralHubBuilderConfiguration
{
    public static NeuralHubBuilder AddYolo5Models(this NeuralHubBuilder neuralHub, int count)
    {
        foreach (var _ in Enumerable.Range(0, count))
        {
            neuralHub.AddModel<HelloWorldModel, StringToStringTask, HelloWorldOptions>(new HelloWorldOptions());
        }
        
        return neuralHub;
    } 
}
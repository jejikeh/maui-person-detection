using FluentAssertions;
using Neural.Defaults;
using Neural.Tests.Common.Mocks.Samples.HelloNumberCluster.Configuration;
using Neural.Tests.Common.Mocks.Samples.HelloNumberCluster.Models;
using Neural.Tests.Common.Mocks.Samples.HelloNumberCluster.Tasks.IntToString;
using Neural.Tests.Common.Mocks.Samples.SumOfNumbersCluster.Configuration;
using Neural.Tests.Common.Mocks.Samples.SumOfNumbersCluster.Models;
using Neural.Tests.Common.Mocks.Samples.SumOfNumbersCluster.Tasks.IntToInt;
using Neural.Tests.Common.Utils;

namespace Neural.Core.Tests;

public class PipesTests
{
    [Fact]
    public async void GivenTwoClusters_WhenPipeOutputToCluster_ThenOutputIsConvertedToInput()
    {
        // Arrange
        var neuralHub = NeuralHubConfiguration
            .FromDefaults()
            .AddSumNumbersModels(3)
            .AddHelloNumberModels(3)
            .Build();
        
        var sumCluster = neuralHub.ShapeCluster<SumNumbersModel, IntsToIntTask>();
        var helloCluster = neuralHub.ShapeCluster<HelloNumberModel, IntToStringTask>();

        var output = string.Empty;
        
        // Act & Assert
        await sumCluster.RunHandleAsync(
            FakeData.IntsToIntTask(10),
            async intsToIntTask =>
                await helloCluster.RunHandleAsync(
                    IntToStringTask.FromIntOutput(intsToIntTask.IntOutput()),
                    intToStringTask =>
                    {
                        output = intToStringTask.StringOutput().Value;
                    }));
        
        output.Should().Be("Hello 45");
    }
}
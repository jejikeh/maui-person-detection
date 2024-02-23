using FluentAssertions;
using Neural.Core.Tests.Mocks;
using Neural.Defaults;
using Neural.Defaults.Models;
using Neural.Tests.Common.Mocks.Samples.HelloNumberCluster.Configuration;
using Neural.Tests.Common.Mocks.Samples.HelloNumberCluster.Models;
using Neural.Tests.Common.Mocks.Samples.HelloNumberCluster.Tasks.IntToString;
using Neural.Tests.Common.Mocks.Samples.SumOfNumbersCluster.Configuration;
using Neural.Tests.Common.Mocks.Samples.SumOfNumbersCluster.Models;
using Neural.Tests.Common.Mocks.Samples.SumOfNumbersCluster.Tasks.IntToInt;
using Neural.Tests.Common.Utils;

namespace Neural.Core.Tests;

public class PipelineTests
{
    [Fact]
    public void GivenTwoClusters_WhenChainOutputToCluster_ThenOutputIsConvertedToInput()
    {
        // Arrange
        var neuralHub = NeuralHubConfiguration
            .FromDefaults()
            .AddSumNumbersModelsMocks(3)
            .AddHelloNumberModelsMocks(3)
            .Build();
        
        var sumCluster = neuralHub.ShapeCluster<Cluster<SumNumbersModel, IntsToIntTask>>();
        var helloCluster = neuralHub.ShapeCluster<Cluster<HelloNumberModel, IntToStringTask>>();

        var output = string.Empty;
        var handleTimes = 0;
        
        // Act
        var cluster = sumCluster?.RunHandleAsync(
            FakeData.IntsToIntTask(10),
            async intsToIntTask =>
                await helloCluster?.RunHandleAsync(
                    IntToStringTask.FromTask(intsToIntTask),
                    async intToStringTask =>
                    {
                        output = intToStringTask.StringOutput().Value;
                        handleTimes++;
                    }));

        cluster?.Wait();
        
        // Assert
        output.Should().Be("Hello 45");
        handleTimes.Should().Be(1);
    }
    
    [Fact]
    public void GivenTwoClustersAndEnumerableAsInput_WhenChainOutputToCluster_ThenOutputIsConvertedToInput()
    {
        // Arrange
        var neuralHub = NeuralHubConfiguration
            .FromDefaults()
            .AddSumNumbersModelsMocks(3)
            .AddHelloNumberModelsMocks(3)
            .Build();
        
        var sumCluster = neuralHub.ShapeCluster<Cluster<SumNumbersModel, IntsToIntTask>>();
        var helloCluster = neuralHub.ShapeCluster<Cluster<HelloNumberModel, IntToStringTask>>();

        var output = string.Empty;
        var handleTimes = 0;
        
        // Act
        var cluster = sumCluster?.RunHandleAsync(
            FakeData.IntsToIntTasks(10),
            async intsToIntTask =>
                await helloCluster?.RunHandleAsync(
                    IntToStringTask.FromTask(intsToIntTask),
                    async intToStringTask =>
                    {
                        output = intToStringTask.StringOutput().Value;
                        handleTimes++;
                    }));

        cluster?.Wait();
        
        // Assert
        output.Should().Be("Hello 45");
        handleTimes.Should().Be(10);
    }

    [Fact]
    public void GivenManyClusters_WhenExtractPipeline_ThenReturnValidPipeline()
    {
        // Arrange
        var modelsCount = FakeData.IntFromSmallRange();

        var neuralHub = NeuralHubConfiguration
            .FromDefaults()
            .AddSumNumbersModelsMocks(modelsCount)
            .AddHelloNumberModelsMocks(modelsCount)
            .Build();

        // Act
        var story = neuralHub.ExtractPipeline<HelloSumNumberMock>();
        
        // Assert
        story.Should().NotBeNull();
        story!.SumNumbersCluster.Should().NotBeNull();
        story.HelloNumberCluster.Should().NotBeNull();

        story.SumNumbersCluster!.Count().Should().Be(modelsCount);
        story.HelloNumberCluster!.Count().Should().Be(modelsCount);
    }

    [Fact]
    public void GivenStory_WhenRunAsync_ThenReturnOutputFromPipeline()
    {
        // Arrange
        var modelsCount = FakeData.IntFromSmallRange();

        var neuralHub = NeuralHubConfiguration
            .FromDefaults()
            .AddSumNumbersModelsMocks(modelsCount)
            .AddHelloNumberModelsMocks(modelsCount)
            .Build();

        var story = neuralHub.ExtractPipeline<HelloSumNumberMock>();

        // Act
        var output = story!.RunAsync(FakeData.IntsToIntTask(10));
        
        // Assert
        output.Should().NotBeNull();
        output.Result!.StringOutput().Value.Should().Be("Hello 45");
    }
}
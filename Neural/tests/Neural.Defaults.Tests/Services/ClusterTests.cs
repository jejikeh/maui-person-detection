using FluentAssertions;
using Neural.Core.Models;
using Neural.Tests.Common.Mocks.Models.Tasks;
using Neural.Tests.Common.Mocks.Models.Yolo5;
using Neural.Tests.Common.Mocks.Samples.SumOfNumbersCluster.Configuration;
using Neural.Tests.Common.Mocks.Samples.SumOfNumbersCluster.Models;
using Neural.Tests.Common.Mocks.Samples.SumOfNumbersCluster.Tasks.IntToInt;
using Neural.Tests.Common.Utils;

namespace Neural.Defaults.Tests.Services;

public class ClusterTests
{
    [Fact]
    public void GivenModelWithSameTaskInput_WhenShapeCluster_ThenReturnCorrectCluster()
    {
        // Arrange
        var modelsCount = FakeData.IntFromSmallRange();
        
        // Use default Cluster Provider FromDefaults()
        var neuralHub = NeuralHubConfiguration
            .FromDefaults()
            .AddYolo5ModelsStringToString(modelsCount)
            .Build();
        
        // Act
        var yolo5Cluster = neuralHub.ShapeCluster<Yolo5ModelStringToStringMock, StringToStringTaskMock>();
        
        // Assert
        yolo5Cluster.Should().NotBeNull();
        yolo5Cluster.Count().Should().Be(modelsCount);
    }

    [Fact]
    public async void GivenTaskInput_WhenRunAsync_ThenShouldGiveTaskResult()
    {
        // Arrange
        var neuralHub = NeuralHubConfiguration
            .FromDefaults()
            .AddYolo5ModelStringToString()
            .Build();
        
        var cluster = neuralHub.ShapeCluster<Yolo5ModelStringToStringMock, StringToStringTaskMock>();
        
        // Act
        var result = await cluster.RunAsync(FakeData.StringToStringTaskMock);
        
        // Assert
        result.Should().NotBeNull();
        result!.StringOutput.Value.Should().Be(Yolo5ModelStringToStringMock.MockedOutput);
    }
    
    [Fact]
    public async void GivenMultipleTaskInput_WhenRunAsync_ThenReturnModelTask()
    {
        // Arrange
        var neuralHub = NeuralHubConfiguration
            .FromDefaults()
            .AddYolo5ModelsStringToString(3)
            .Build();
        
        var cluster = neuralHub.ShapeCluster<Yolo5ModelStringToStringMock, StringToStringTaskMock>();
        
        // Act
        var resultFirst = await cluster.RunAsync(FakeData.StringToStringTaskMock);
        var resultSecond = await cluster.RunAsync(FakeData.StringToStringTaskMock);
        var resultThird = await cluster.RunAsync(FakeData.StringToStringTaskMock);
        var resultFour = await cluster.RunAsync(FakeData.StringToStringTaskMock);

        // Assert
        resultFirst.Should().NotBeNull();
        resultFirst!.StringOutput.Value.Should().Be(Yolo5ModelStringToStringMock.MockedOutput);
        
        resultSecond.Should().NotBeNull();
        resultSecond!.StringOutput.Value.Should().Be(Yolo5ModelStringToStringMock.MockedOutput);
        
        resultThird.Should().NotBeNull();
        resultThird!.StringOutput.Value.Should().Be(Yolo5ModelStringToStringMock.MockedOutput);
        
        resultFour.Should().NotBeNull();
        resultFour!.StringOutput.Value.Should().Be(Yolo5ModelStringToStringMock.MockedOutput);
    }

    [Fact]
    public async void GivenModelInput_WhenRunInBackground_ThenReturnModelTaskInBackgroundEvent()
    {
        // Arrange
        var neuralHub = NeuralHubConfiguration
            .FromDefaults()
            .AddYolo5ModelsStringToString(3)
            .Build();
        
        var cluster = neuralHub.ShapeCluster<Yolo5ModelStringToStringMock, StringToStringTaskMock>();
        
        // Act
        var resultFirst = await cluster.RunInBackgroundAsync(FakeData.StringToStringTaskMock);
        
        // Assert
        resultFirst.Should().NotBeNull();
        resultFirst!.StringOutput.Value.Should().NotBe(Yolo5ModelStringToStringMock.MockedBackgroundOutput);

        var modelInvoked = false;
        
        resultFirst.OnModelTaskCompleted += (_, _) =>
        {
            modelInvoked = true;
        };

        await Task.Delay(Yolo5ModelStringToStringMock.BackgroundDelayMs + 10);
        
        modelInvoked.Should().BeTrue();
        resultFirst.StringOutput.Value.Should().Be(Yolo5ModelStringToStringMock.MockedBackgroundOutput);
    }

    [Fact]
    public async void GivenMultipleModelInput_WhenRunInBackground_ThenReturnModelTaskInBackgroundEvent()
    {
        // Arrange
        var clusterCount = FakeData.IntFromSmallRange();
        
        var neuralHub = NeuralHubConfiguration
            .FromDefaults()
            .AddYolo5ModelsStringToString(clusterCount)
            .Build();
        
        var cluster = neuralHub.ShapeCluster<Yolo5ModelStringToStringMock, StringToStringTaskMock>();

        var modelCompleted = 0;
        
        // Act
        for (var i = 0; i < clusterCount; i++)
        {
            var task = await cluster.RunInBackgroundAsync(FakeData.StringToStringTaskMock);
            
            task.OnModelTaskCompleted += (_, _) => { modelCompleted++; };
        }
        
        var clusterUnderLoad = cluster.IsAnyModelWithStatus(ModelStatus.Active);
        
        // Assert
        clusterUnderLoad.Should().BeTrue();
        
        await Task.Delay(Yolo5ModelStringToStringMock.BackgroundDelayMs * 2);
        
        modelCompleted.Should().Be(clusterCount);
        
        clusterUnderLoad = cluster.IsAnyModelWithStatus(ModelStatus.Active);
        clusterUnderLoad.Should().BeFalse();
    }

    [Fact]
    public async void GivenModelInput_WhenRunHandleAsync_ThenCallHandleWithModelTask()
    {
        // Assert
        var modelCount = FakeData.IntFromSmallRange();
        var tasksCount = FakeData.IntFromSmallRange();
        var handleCallTimes = 0;

        var neuralHub = NeuralHubConfiguration
            .FromDefaults()
            .AddSumNumbersModels(modelCount)
            .Build();
        
        var cluster = neuralHub.ShapeCluster<SumNumbersModel, IntsToIntTask>();
        
        
        // Act
        await cluster.RunHandleAsync(FakeData.IntsToIntTasks(tasksCount), _ =>
        {
            handleCallTimes++;
        });
        
        // Assert
        handleCallTimes.Should().Be(tasksCount);
    }
}
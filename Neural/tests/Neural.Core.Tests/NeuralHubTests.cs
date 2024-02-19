using FluentAssertions;
using Neural.Core.Models;
using Neural.Core.Tests.Mocks.Models;
using Neural.Core.Tests.Mocks.Services;
using Neural.Defaults;
using Neural.Tests.Common.Mocks.Models.Tasks;
using Neural.Tests.Common.Mocks.Models.Yolo5;
using Neural.Tests.Common.Mocks.Models.Yolo8;
using Neural.Tests.Common.Utils;

namespace Neural.Core.Tests;

public class NeuralHubTests
{
    [Fact]
    public void GivenModel_WhenGetModel_ThenReturnModel()
    {
        // Arrange
        var neuralHub = NeuralHubConfiguration
            .FromDefaults()
            .AddYolo5ModelStringToString()
            .Build();
        
        // Act
        var model = neuralHub.GetModels<Yolo5ModelStringToStringMock>().First();
        
        // Assert
        model.Should().BeOfType<Yolo5ModelStringToStringMock>();
    }

    [Fact]
    public void GivenModels_WhenGetModel_ThenReturnAllModel()
    {
        // Arrange
        var neuralHub = NeuralHubConfiguration
            .FromDefaults()
            .AddYolo5ModelStringToString()
            .AddYolo8ModelStringToString()
            .AddYolo8ModelIntToString()
            .AddYolo8ModelIntToString()
            .Build();
        
        // Act
        var yolo5Model = neuralHub.GetModels<Yolo5ModelStringToStringMock>();
        var yolo8WithOptionsModel = neuralHub.GetModels<Yolo8ModelStringToStringMock>();
        var yolo8Model = neuralHub.GetModels<Yolo8ModelIntToStringMock>();
        
        // Assert
        yolo5Model.Should().HaveCount(1);
        yolo8WithOptionsModel.Should().HaveCount(1);
        yolo8Model.Should().HaveCount(2);
    }
    
    [Fact]
    public async void GivenModelTask_WhenRunAsync_ThenReturnCorrectModelOutput()
    {
        // Arrange
        var modelInputs = FakeData.StringToStringTaskMock;
        
        var neuralHub = NeuralHubConfiguration
            .FromDefaults()
            .AddYolo5ModelStringToString()
            .Build();
        
        var yolo5Model = neuralHub.GetModels<Yolo5ModelStringToStringMock>().First();

        // Act
        var modelOutputs = await yolo5Model.RunAsync(modelInputs);
        
        // Assert
        modelOutputs.StringOutput.Value.Should().Be(Yolo5ModelStringToStringMock.MockedOutput);
    }

    [Fact]
    public async void GivenModelTask_WhenHubRunAsync_ThenReturnCorrectModelOutput()
    {
        // Arrange
        var modelInputs = FakeData.StringToStringTaskMock;
        
        var neuralHub = NeuralHubConfiguration
            .FromDefaults()
            .AddYolo5ModelStringToString()
            .Build();
        
        // Act
        var hubOutputs = await neuralHub.RunAsync(modelInputs);
        
        // Assert
        hubOutputs.Should().NotBeNull();
        hubOutputs!.StringOutput.Value.Should().Be(Yolo5ModelStringToStringMock.MockedOutput);
    }
    
    [Fact]
    public async void GivenModelTaskWhichDoesntContainInHub_WhenHubRunAsync_ThenReturnNull()
    {
        // In this test we load the Yolo8Model with input task InputToStringTaskMock.
        // However, in RunAsync we load the StringToStringTaskModelMock, which is not any model in the hub a accepts.
        
        // Arrange
        var neuralHub = NeuralHubConfiguration
            .FromDefaults()
            .AddYolo8ModelIntToString()
            .Build();
        
        // Act
        var hubOutputs = await neuralHub.RunAsync(FakeData.StringToStringTaskMock);
        
        // Assert
        hubOutputs.Should().BeNull();
    }

    [Fact]
    public async void GivenModelTaskAndSpecifyTheModel_WhenHubRunAsync_ThenReturnCorrectModelOutputFromSpecifiedModel()
    {
        // Arrange
        var neuralHub = NeuralHubConfiguration
            .FromDefaults()
            .AddYolo5ModelStringToString()
            .Build();
        
        // Act
        var yolo5ModelOutputs = await neuralHub
            .RunAsync<Yolo5ModelStringToStringMock, StringToStringTaskMock>(FakeData.StringToStringTaskMock);
        
        // Assert
        var yolo5Model = neuralHub.GetModels<Yolo5ModelStringToStringMock>().First();
        yolo5Model.Status.Should().Be(ModelStatus.Inactive);
        
        yolo5ModelOutputs.Should().NotBeNull();
        yolo5ModelOutputs!.StringOutput.Value.Should().Be(Yolo5ModelStringToStringMock.MockedOutput);
    }
    
    [Fact]
    public async void GivenTheSameInput_WhenMultipleHubRunAsync_ThenChainTheSameInput()
    {
        // Arrange
        var modelsInput = FakeData.StringToStringTaskMock;
        
        var neuralHub = NeuralHubConfiguration
            .FromDefaults()
            .AddYolo5ModelStringToString()
            .Build();
        
        // Act
        var yolo5ModelOutputs = await neuralHub
            .RunAsync<Yolo5ModelStringToStringMock, StringToStringTaskMock>(modelsInput);
        
        // Assert
        yolo5ModelOutputs.Should().NotBeNull();
        yolo5ModelOutputs!.StringOutput.Value.Should().Be(Yolo5ModelStringToStringMock.MockedOutput);
    }
    
    [Fact]
    public async void GivenModelTask_WhenBackgroundRunAsync_ThenReturnCorrectModelOutputGetFromEvent()
    {
        // Arrange
        var modelsInput = FakeData.StringToStringTaskMock;
        
        var neuralHub = NeuralHubConfiguration
            .FromDefaults()
            .AddYolo5ModelStringToString()
            .Build();
        
        // Act
        var modelOutput = neuralHub.TryRunInBackground(modelsInput);
        
        // Assert
        modelOutput.Should().NotBeNull();

        var yolo5Model = neuralHub.GetModels<Yolo5ModelStringToStringMock>().FirstOrDefault();
        yolo5Model!.Status.Should().Be(ModelStatus.Active);
        
        var eventInvoked = false;
        modelOutput!.OnModelTaskCompleted += (_, _) =>
        {
            eventInvoked = true;
        };
        
        eventInvoked.Should().BeFalse();
        
        // Yolo5ModelMock.TryRunInBackground will delay 100ms for testing purpose
        await Task.Delay(110);
        
        eventInvoked.Should().BeTrue();
        
        modelOutput.StringOutput.Value.Should().Be(Yolo5ModelStringToStringMock.MockedBackgroundOutput);
    }
    
    [Fact]
    public async void GivenModelTasksMoreThanOneModels_WhenRunInBackground_ThenAllModelsShouldStartProcessingTask()
    {
        // Arrange
        var neuralHub = NeuralHubConfiguration
            .FromDefaults()
            .AddYolo5ModelStringToString()
            .AddYolo5ModelStringToString()
            .Build();
        
        // Act
        var modelEventFirst = neuralHub.TryRunInBackground(FakeData.StringToStringTaskMock);
        var modelEventSecond = neuralHub.TryRunInBackground(FakeData.StringToStringTaskMock);
        var modelEventThird = neuralHub.TryRunInBackground(FakeData.StringToStringTaskMock);

        // Assert
        modelEventFirst.Should().NotBeNull();
        modelEventSecond.Should().NotBeNull();
        
        // Added only 2 StringToString type models, so all them should be active
        modelEventThird.Should().BeNull();
        
        var models = neuralHub.GetModels<Yolo5ModelStringToStringMock>().ToArray();
        models.Length.Should().Be(2);
        
        models.ShouldHaveState(ModelStatus.Active);
        
        await Task.Delay(110);
        
        models.ShouldHaveState(ModelStatus.Inactive);
    }

    [Fact]
    public void GivenNonDefaultClusterProvider_WhenShapeCluster_ThenShapeMockedCluster()
    {
        // Arrange
        var modelCount = FakeData.IntFromSmallRange();
        
        var neuralHub = NeuralHubConfiguration
            .FromDefaults(
                clusterProvider: new ClusterProviderMock())
            .AddYolo5ModelsStringToString(modelCount)
            .Build();
        
        // Act
        var cluster = neuralHub.ShapeCluster<Yolo5ModelStringToStringMock, StringToStringTaskMock>();
        
        // Assert
        cluster.Should().BeOfType<ClusterMock<Yolo5ModelStringToStringMock, StringToStringTaskMock>>();
        cluster.Count().Should().Be(modelCount);
    }
    
    [Fact]
    public void GivenNonDefaultClusterProvider_WhenShapeCluster_ThenUseNonDefaultProvider()
    {
        // Arrange
        var modelCount = FakeData.IntFromSmallRange();
        
        // DoubleClusterProviderMock will be used for testing purpose to check if non default cluster provider is used
        // DoubleClusterProviderMock should double the model count
        var neuralHub = NeuralHubConfiguration
            .FromDefaults(
                clusterProvider: new DoubleCountClusterProviderMock())
            .AddYolo5ModelsStringToString(modelCount)
            .Build();
        
        // Act
        var cluster = neuralHub.ShapeCluster<Yolo5ModelStringToStringMock, StringToStringTaskMock>();
        
        // Assert
        cluster.Should().BeOfType<ClusterMock<Yolo5ModelStringToStringMock, StringToStringTaskMock>>();
        cluster.Count().Should().Be(modelCount * 2);
    }
}
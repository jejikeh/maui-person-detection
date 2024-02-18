using FluentAssertions;
using Neural.Core.Models;
using Neural.Defaults;
using Neural.Tests.Common.Mocks.Models.Tasks;
using Neural.Tests.Common.Mocks.Models.Yolo5;
using Neural.Tests.Common.Mocks.Models.Yolo8;
using Neural.Tests.Common.Mocks.Options;
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
            .AddModel<Yolo5ModelMock, StringToStringTaskMock>(Yolo5Options.ModelPath)
            .Build();
        
        // Act
        var model = neuralHub.GetModels<Yolo5ModelMock>().First();
        
        // Assert
        model.Should().BeOfType<Yolo5ModelMock>();
    }

    [Fact]
    public void GivenModels_WhenGetModel_ThenReturnAllModel()
    {
        // Arrange
        var neuralHub = NeuralHubConfiguration
            .FromDefaults()
            .AddYolo5Model()
            .AddYolo8ModelWithOptions()
            .AddYolo8Model()
            .AddYolo8Model()
            .Build();
        
        // Act
        var yolo5Model = neuralHub.GetModels<Yolo5ModelMock>();
        var yolo8WithOptionsModel = neuralHub.GetModels<Yolo8ModelWithOptionsMock>();
        var yolo8Model = neuralHub.GetModels<Yolo8ModelMock>();
        
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
            .AddModel<Yolo5ModelMock, StringToStringTaskMock>(Yolo5Options.ModelPath)
            .Build();
        
        var yolo5Model = neuralHub.GetModels<Yolo5ModelMock>().First();

        // Act
        var modelOutputs = await yolo5Model.RunAsync(modelInputs);
        
        // Assert
        modelOutputs.StringOutput.Value.Should().Be(Yolo5ModelMock.MockedOutput);
    }

    [Fact]
    public async void GivenModelTask_WhenHubRunAsync_ThenReturnCorrectModelOutput()
    {
        // Arrange
        var modelInputs = FakeData.StringToStringTaskMock;
        
        var neuralHub = NeuralHubConfiguration
            .FromDefaults()
            .AddModel<Yolo5ModelMock, StringToStringTaskMock>(Yolo5Options.ModelPath)
            .Build();
        
        // Act
        var hubOutputs = await neuralHub.RunAsync(modelInputs);
        
        // Assert
        hubOutputs.Should().NotBeNull();
        hubOutputs!.StringOutput.Value.Should().Be(Yolo5ModelMock.MockedOutput);
    }
    
    [Fact]
    public async void GivenModelTaskWhichDoesntContainInHub_WhenHubRunAsync_ThenReturnNull()
    {
        // In this test we load the Yolo8Model with input task InputToStringTaskMock.
        // However, in RunAsync we load the StringToStringTaskModelMock, which is not any model in the hub a accepts.
        
        // Arrange
        var neuralHub = NeuralHubConfiguration
            .FromDefaults()
            .AddModel<Yolo8ModelMock, IntToStringTaskMock>(Yolo5Options.ModelPath)
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
            .AddYolo5Model()
            .AddYolo5ModelWithOptions()
            .Build();
        
        // Act
        var yolo5ModelOutputs = await neuralHub
            .RunAsync<Yolo5ModelMock, StringToStringTaskMock>(FakeData.StringToStringTaskMock);
        
        var yolo5ModelWithOptionsOutputs = await neuralHub
            .RunAsync<Yolo5ModelWithOptionsMock, StringToStringTaskMock>(FakeData.StringToStringTaskMock);
        
        // Assert
        var yolo5Model = neuralHub.GetModels<Yolo5ModelMock>().First();
        yolo5Model.Status.Should().Be(ModelStatus.Inactive);
        
        var yolo5ModelWithOptions = neuralHub.GetModels<Yolo5ModelWithOptionsMock>().First();
        yolo5ModelWithOptions.Status.Should().Be(ModelStatus.Inactive);
        
        yolo5ModelOutputs.Should().NotBeNull();
        yolo5ModelOutputs!.StringOutput.Value.Should().Be(Yolo5ModelMock.MockedOutput);
        
        yolo5ModelWithOptionsOutputs.Should().NotBeNull();
        yolo5ModelWithOptionsOutputs!.StringOutput.Value.Should().Be(Yolo5ModelWithOptionsMock.MockedOutput);
    }
    
    [Fact]
    public async void GivenTheSameInput_WhenMultipleHubRunAsync_ThenChainTheSameInput()
    {
        // Arrange
        var modelsInput = FakeData.StringToStringTaskMock;
        
        var neuralHub = NeuralHubConfiguration
            .FromDefaults()
            .AddYolo5Model()
            .AddYolo5ModelWithOptions()
            .Build();
        
        // Act
        var yolo5ModelOutputs = await neuralHub
            .RunAsync<Yolo5ModelMock, StringToStringTaskMock>(modelsInput);
        
        // Assert
        yolo5ModelOutputs.Should().NotBeNull();
        yolo5ModelOutputs!.StringOutput.Value.Should().Be(Yolo5ModelMock.MockedOutput);
        
        // Act
        var yolo5ModelWithOptionsOutputs = await neuralHub
            .RunAsync<Yolo5ModelWithOptionsMock, StringToStringTaskMock>(modelsInput);
        
        // Assert
        yolo5ModelOutputs.Should().NotBeNull();
        yolo5ModelOutputs.StringOutput.Value.Should().Be(Yolo5ModelWithOptionsMock.MockedOutput);
        
        yolo5ModelWithOptionsOutputs.Should().NotBeNull();
        yolo5ModelWithOptionsOutputs!.StringOutput.Value.Should().Be(Yolo5ModelWithOptionsMock.MockedOutput);
    }
    
    [Fact]
    public async void GivenModelTask_WhenBackgroundRunAsync_ThenReturnCorrectModelOutputGetFromEvent()
    {
        // Arrange
        var modelsInput = FakeData.StringToStringTaskMock;
        
        var neuralHub = NeuralHubConfiguration
            .FromDefaults()
            .AddYolo5Model()
            .Build();
        
        // Act
        var modelOutput = neuralHub.TryRunInBackground(modelsInput);
        
        // Assert
        modelOutput.Should().NotBeNull();

        var yolo5Model = neuralHub.GetModels<Yolo5ModelMock>().FirstOrDefault();
        yolo5Model!.Status.Should().Be(ModelStatus.Active);
        
        var eventInvoked = false;
        modelOutput!.OnModelTaskCompleted += _ =>
        {
            eventInvoked = true;
        };
        
        eventInvoked.Should().BeFalse();
        
        // Yolo5ModelMock.TryRunInBackground will delay 100ms for testing purpose
        await Task.Delay(110);
        
        eventInvoked.Should().BeTrue();
        
        modelOutput.StringOutput.Value.Should().Be(Yolo5ModelMock.MockedBackgroundOutput);
    }
    
    [Fact]
    public void GivenModelTasksMoreThanOneModels_WhenRunInBackground_ThenAllModelsShouldStartProcessingTask()
    {
        // Arrange
        var neuralHub = NeuralHubConfiguration
            .FromDefaults()
            .AddYolo5Model()
            .AddYolo5Model()
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
    }
}
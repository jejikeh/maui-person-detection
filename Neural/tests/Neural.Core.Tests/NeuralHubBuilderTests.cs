using FluentAssertions;
using Neural.Defaults;
using Neural.Tests.Common.Mocks.Exceptions;
using Neural.Tests.Common.Mocks.Models.Tasks;
using Neural.Tests.Common.Mocks.Models.Yolo5;
using Neural.Tests.Common.Mocks.Models.Yolo8;
using Neural.Tests.Common.Mocks.Options;
using Neural.Tests.Common.Mocks.Services;
using Neural.Tests.Common.Utils;

namespace Neural.Core.Tests;

public class NeuralHubBuilderTests
{
    [Fact]
    public void GivenModelProviders_WhenBuildHub_ThenLoadNeuralHub()
    {
        // Arrange
        var neuralHubBuilder = NeuralHubConfiguration
            .FromDefaults()
            .AddModel<Yolo5ModelWithOptionsMock, StringToStringTaskMock, Yolo5Options>(new Yolo5Options())
            .AddModel<Yolo5ModelMock, StringToStringTaskMock>(Yolo5Options.ModelPath)
            .AddModel<Yolo8ModelWithOptionsMock, StringToStringTaskMock, Yolo8QuantizedOptions>(new Yolo8QuantizedOptions());

        // Act
        var neuralHub = neuralHubBuilder.Build();

        // Assert
        neuralHub.Should().NotBeNull();
        neuralHub.Models.Count.Should().Be(3);
        
        var sortedModels = neuralHub.Models
            .OrderBy(model => model.GetType().Name)
            .ToList();
        
        var yolo5Model = sortedModels[0];
        yolo5Model.Should().BeOfType<Yolo5ModelMock>();
        
        var yolo5ModelWithOptions = sortedModels[1];
        yolo5ModelWithOptions.Should().BeOfType<Yolo5ModelWithOptionsMock>();
        
        var yolo8ModelWithOptions = sortedModels[2];
        yolo8ModelWithOptions.Should().BeOfType<Yolo8ModelWithOptionsMock>();
    }

    [Fact]
    public void GivenModelProviders_WhenBuildHub_ThenLoadNeuralInstances()
    {
        // Arrange
        var neuralHubBuilder = NeuralHubConfiguration
            .FromDefaults()
            .AddModel<Yolo5ModelWithOptionsMock, StringToStringTaskMock, Yolo5Options>(new Yolo5Options())
            .AddModel<Yolo8ModelWithOptionsMock, StringToStringTaskMock, Yolo8QuantizedOptions>(new Yolo8QuantizedOptions());
        
        // Act
        var neuralHub = neuralHubBuilder.Build();
        
        // Assert
        neuralHub.Models.Count.Should().Be(2);
    }

    [Fact]
    public void GivenModelProviders_WhenBuildHub_ThenLoadModelInstanceFile()
    {
        // Arrange
        var neuralHubBuilder = NeuralHubConfiguration
            .FromDefaults()
            .AddModel<Yolo8ModelWithOptionsMock, StringToStringTaskMock, Yolo8QuantizedOptions>(new Yolo8QuantizedOptions());
        
        // Act
        var neuralHub = neuralHubBuilder.Build();
        
        // Assert
        var loadedModel = neuralHub.Models[0];
        
        loadedModel.Should().NotBeNull();
        loadedModel.Should().BeOfType<Yolo8ModelWithOptionsMock>();
        loadedModel.InferenceSession.Should().NotBeNull();
    }

    [Fact]
    public void GivenNonExistingModelPath_WhenBuildHub_ThenThrowException()
    {
        // Arrange
        var neuralHubBuilder = NeuralHubConfiguration
            .FromDefaults()
            .AddModel<Yolo5ModelMock, StringToStringTaskMock>(FakeData.FilePath());
        
        // Act
        var act = () => neuralHubBuilder.Build();
        
        // Assert
        act.Should().Throw<Exception>(); 
    }

    [Fact]
    public void GivenNonDefaultServices_WhenBuildHub_ThenLoadNeuralHubUsingCustomServices()
    {
        // Arrange
        var neuralHubBuilder = NeuralHubConfiguration
            .FromDefaults(
                modelProvider: new ExceptionModelProviderMock())
            .AddModel<Yolo5ModelMock, StringToStringTaskMock>("yolo5n.onnx");
        
        // Act
        var act = () => neuralHubBuilder.Build();
        
        // Assert
        act.Should().Throw<FakeException>();
    }
}
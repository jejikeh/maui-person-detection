using FluentAssertions;
using Neural.Core.Tests.Mocks.Exceptions;
using Neural.Core.Tests.Mocks.Models.Yolo5;
using Neural.Core.Tests.Mocks.Models.Yolo8;
using Neural.Core.Tests.Mocks.Options;
using Neural.Core.Tests.Mocks.Services;
using Neural.Core.Tests.Utils;
using Neural.Defaults;

namespace Neural.Core.Tests;

public class NeuralHubBuilderTests
{
    [Fact]
    public async void GivenModelProviders_WhenBuildHub_ThenLoadNeuralHub()
    {
        // Arrange
        var neuralHubBuilder = NeuralHubConfiguration
            .FromDefaults()
            .AddModel<Yolo5ModelWithOptionsMock, Yolo5Options>(new Yolo5Options())
            .AddModel<Yolo8ModelWithOptionsMock, Yolo8QuantizedOptions>(new Yolo8QuantizedOptions());

        // Act
        var neuralHub = await neuralHubBuilder.BuildAsync();

        // Assert
        neuralHub.Should().NotBeNull();
        neuralHub.Models.Count.Should().Be(2);
        
        var yolo5Model = neuralHub.Models[0];
        yolo5Model.Should().BeOfType<Yolo5ModelWithOptionsMock>();
        
        var yolo8Model = neuralHub.Models[1];
        yolo8Model.Should().BeOfType<Yolo8ModelWithOptionsMock>();
    }

    [Fact]
    public async void GivenModelProviders_WhenBuildHub_ThenLoadNeuralInstances()
    {
        // Arrange
        var neuralHubBuilder = NeuralHubConfiguration
            .FromDefaults()
            .AddModel<Yolo5ModelWithOptionsMock, Yolo5Options>(new Yolo5Options())
            .AddModel<Yolo8ModelWithOptionsMock, Yolo8QuantizedOptions>(new Yolo8QuantizedOptions());
        
        // Act
        var neuralHub = await neuralHubBuilder.BuildAsync();
        
        // Assert
        neuralHub.Models.Count.Should().Be(2);
    }

    [Fact]
    public async void GivenModelProviders_WhenBuildHub_ThenLoadModelInstanceFile()
    {
        // Arrange
        var neuralHubBuilder = NeuralHubConfiguration
            .FromDefaults()
            .AddModel<Yolo8ModelWithOptionsMock, Yolo8QuantizedOptions>(new Yolo8QuantizedOptions());
        
        // Act
        var neuralHub = await neuralHubBuilder.BuildAsync();
        
        // Assert
        var loadedModel = neuralHub.Models[0];
        
        loadedModel.Should().NotBeNull();
        loadedModel.Should().BeOfType<Yolo8ModelWithOptionsMock>();
        loadedModel.InferenceSession.Should().NotBeNull();
    }

    [Fact]
    public async void GivenNonExistingModelProviders_WhenBuildHub_ThenThrowException()
    {
        // Arrange
        var neuralHubBuilder = NeuralHubConfiguration
            .FromDefaults()
            .AddModel<Yolo5ModelMock>(FakeData.FilePath());
        
        // Act
        var act = async () => await neuralHubBuilder.BuildAsync();
        
        // Assert
        await act.Should().ThrowAsync<Exception>(); 
    }

    [Fact]
    public void GivenNonDefaultServices_WhenBuildHub_ThenLoadNeuralHubUsingCustomServices()
    {
        // Arrange
        var neuralHubBuilder = NeuralHubConfiguration
            .FromDefaults(
                modelProvider: new ExceptionModelProviderMock())
            .AddModel<Yolo5ModelMock>("yolo5n.onnx");
        
        // Act
        var act = async () => await neuralHubBuilder.BuildAsync();
        
        // Assert
        act.Should().ThrowAsync<FakeException>();
    }
}
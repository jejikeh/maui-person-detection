using FluentAssertions;
using Neural.Defaults;
using Neural.Defaults.Common.Dependencies;
using Neural.Tests.Common.Mocks;
using Neural.Tests.Common.Mocks.Exceptions;
using Neural.Tests.Common.Mocks.Models.Tasks;
using Neural.Tests.Common.Mocks.Models.Yolo5;
using Neural.Tests.Common.Mocks.Models.Yolo8;
using Neural.Tests.Common.Mocks.Services;
using Neural.Tests.Common.Utils;

namespace Neural.Core.Tests;

public class NeuralHubBuilderTests
{
    [Fact]
    public void GivenModelProviders_WhenBuildHub_ThenLoadNeuralHub()
    {
        // Arrange
        var neuralHubBuilder = NeuralHubConfiguration.FromDefaults();
            
        neuralHubBuilder.AddModel<Yolo5ModelStringToStringMock, StringToStringTaskMock, OnnxDependencies>(
                OnnxDependencies.FromBuilder(neuralHubBuilder, Constants.Yolo5ModelPath));
            
        neuralHubBuilder.AddModel<Yolo5ModelStringToStringMock, StringToStringTaskMock, OnnxDependencies>(
                OnnxDependencies.FromBuilder(neuralHubBuilder, Constants.Yolo5ModelPath));
            
        neuralHubBuilder.AddModel<Yolo8ModelStringToStringMock, StringToStringTaskMock, OnnxDependencies>(
            OnnxDependencies.FromBuilder(neuralHubBuilder, Constants.Yolo8ModelPath));

        // Act
        var neuralHub = neuralHubBuilder.Build();

        // Assert
        neuralHub.Should().NotBeNull();
        neuralHub.Models.Count.Should().Be(3);
        
        var sortedModels = neuralHub.Models
            .OrderBy(model => model.GetType().Name)
            .ToList();
        
        var yolo5Model = sortedModels[0];
        yolo5Model.Should().BeOfType<Yolo5ModelStringToStringMock>();
        
        var yolo5ModelWithOptions = sortedModels[1];
        yolo5ModelWithOptions.Should().BeOfType<Yolo5ModelStringToStringMock>();
        
        var yolo8ModelWithOptions = sortedModels[2];
        yolo8ModelWithOptions.Should().BeOfType<Yolo8ModelStringToStringMock>();
    }

    [Fact]
    public void GivenModelProviders_WhenBuildHub_ThenLoadNeuralInstances()
    {
        // Arrange
        var neuralHubBuilder = NeuralHubConfiguration
            .FromDefaults()
            .AddYolo5Models(2);
        
        // Act
        var neuralHub = neuralHubBuilder.Build();
        
        // Assert
        neuralHub.Models.Count.Should().Be(2);
    }

    [Fact]
    public async void GivenNonExistingModelPath_WhenBuildHub_ThenThrowException()
    {
        // Arrange
        var neuralHubBuilder = NeuralHubConfiguration.FromDefaults();
        
        // Act
        var act = async () => neuralHubBuilder
            .AddModel<Yolo5ModelStringToStringMock, StringToStringTaskMock, OnnxDependencies>(
                await OnnxDependencies.FromBuilderAsync(neuralHubBuilder, FakeData.FilePath()));;
        
        // Assert
        await act.Should().ThrowAsync<Exception>(); 
    }

    [Fact]
    public async void GivenNonDefaultServices_WhenBuildHub_ThenLoadNeuralHubUsingCustomServices()
    {
        // Arrange
        var neuralHubBuilder = NeuralHubConfiguration.FromDefaults(modelProvider: new ExceptionModelProviderMock());
        
        neuralHubBuilder
            .AddModel<Yolo5ModelStringToStringMock, StringToStringTaskMock, OnnxDependencies>(
                await OnnxDependencies.FromBuilderAsync(neuralHubBuilder, Constants.Yolo5ModelPath));
        
        // Act
        var act = () => neuralHubBuilder.Build();
        
        // Assert
        act.Should().Throw<FakeException>();
    }
}
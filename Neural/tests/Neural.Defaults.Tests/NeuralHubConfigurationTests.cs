using FluentAssertions;
using Moq;
using Neural.Core.Services;
using Neural.Defaults.Services;
using Neural.Onnx.Common.Dependencies;
using Neural.Tests.Common.Mocks.Models.Tasks;
using Neural.Tests.Common.Mocks.Models.Yolo5;
using Neural.Tests.Common.Utils;

namespace Neural.Defaults.Tests;

public class NeuralHubConfigurationTests
{
    [Fact]
    public void WhenFromDefaults_ThenNeuralHubConfigurationIsNotNull()
    {
        // Arrange & Act
        var neuralHubConfiguration = NeuralHubConfiguration.FromDefaults();
        
        // Assert
        neuralHubConfiguration.Should().NotBeNull();
    }

    [Fact]
    public void GivenOverrideToDefaultServices_WhenFromDefaults_ThenOverrideServicesShouldBeCalled()
    {
        // Arrange
        var fileProvider = new OpenReadFileSystemProvider();
        var modelProviderMock = new Mock<IModelProvider>();
        
        // Act
        var builder = NeuralHubConfiguration.FromDefaults(
                fileSystemProvider: fileProvider,
                modelProvider: modelProviderMock.Object)
            .AddYolo5ModelStringToString()
            .Build();
        
        // Assert
        modelProviderMock.Verify(
            m => m.Initialize<Yolo5ModelStringToStringMock, StringToStringTaskMock, OnnxDependencies>(
                It.IsAny<OnnxDependencies>()), Times.Once);
    }
}
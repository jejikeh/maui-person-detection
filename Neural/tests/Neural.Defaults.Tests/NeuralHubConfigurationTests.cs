using FluentAssertions;
using Moq;
using Neural.Core.Models;
using Neural.Core.Services;
using Neural.Defaults.Services;
using Neural.Tests.Common.Mocks.Models.Tasks;
using Neural.Tests.Common.Mocks.Models.Yolo5;
using Neural.Tests.Common.Mocks.Options;

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
        NeuralHubConfiguration.FromDefaults(
                fileSystemProvider: fileProvider,
                modelProvider: modelProviderMock.Object)
            .AddModel<Yolo5ModelMock, StringToStringTaskMock>(Yolo5Options.ModelPath)
            .Build();
        
        // Assert
        modelProviderMock.Verify(m => m.InitializeAsync<Yolo5ModelMock, StringToStringTaskMock>(fileProvider, Yolo5Options.ModelPath), Times.Once);
    }
}
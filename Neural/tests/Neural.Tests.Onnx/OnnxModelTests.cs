using FluentAssertions;
using Neural.Defaults;
using Neural.Onnx.Common;
using Neural.Tests.Common.Mocks;
using Neural.Tests.Common.Utils;
using Neural.Tests.Onnx.Mocks.Models;
using Neural.Tests.Onnx.Mocks.Tasks;

namespace Neural.Tests.Onnx;

public class OnnxModelTests
{
    [Fact]
    public void GivenNonExistingPath_WhenAddOnnxModelToBuilder_ThenShouldThrowException()
    {
        // Arrange
        var modelCount = FakeData.IntFromSmallRange();
        
        // Act
        var act = () => NeuralHubConfiguration
            .FromDefaults()
            .AddOnnxModels<Yolo5ModelMock, IntToStringTaskMock>(FakeData.FilePath(),  modelCount);
        
        // Assert
        act.Should().Throw<Exception>();
    }
    
    [Fact]
    public void GivenOnnxModel_WhenAddOnnxModelToBuilder_ThenInitializeOnnxModel()
    {
        // Arrange
        var modelCount = FakeData.IntFromSmallRange();
        
        var neuralHubConfiguration = NeuralHubConfiguration
            .FromDefaults()
            .AddOnnxModels<Yolo5ModelMock, IntToStringTaskMock>(Paths.Yolo5ModelPath, modelCount)
            .AddOnnxModels<Yolo8ModelMock, IntToStringTaskMock>(Paths.Yolo8ModelPath, modelCount);

        // Act
        var neuralHub = neuralHubConfiguration.Build();
        
        // Assert
        neuralHub.Models.Count.Should().Be(modelCount * 2);
    }
}
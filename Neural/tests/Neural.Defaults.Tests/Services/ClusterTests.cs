using FluentAssertions;
using Neural.Tests.Common.Mocks.Models.Yolo5;
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
            .AddYolo5Models(modelsCount)
            .Build();
        
        // Act
        var yolo5Cluster = neuralHub.ShapeCluster<Yolo5ModelMock>();
        
        // Assert
        yolo5Cluster.Should().NotBeNull();
        yolo5Cluster.Count().Should().Be(modelsCount);
    }
}
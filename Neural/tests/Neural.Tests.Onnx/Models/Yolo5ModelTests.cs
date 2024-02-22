using FluentAssertions;
using Neural.Defaults;
using Neural.Defaults.Models;
using Neural.Onnx.Common;
using Neural.Onnx.Models.Yolo5;
using Neural.Onnx.Tasks.ImageToBoxPredictions;
using Neural.Tests.Common.Mocks;
using Neural.Tests.Common.Utils;
using Neural.Tests.Onnx.Utils;

namespace Neural.Tests.Onnx.Models;

public class Yolo5ModelTests
{
    [Fact]
    public async void GivenImage_WhenRunAsync_ThenShouldReturnBoxPredictions()
    {
        // Arrange
        var neuralHub = NeuralHubConfiguration
            .FromDefaults()
            .AddOnnxModel<Yolo5Model, ImageToBoxPredictionsYolo5Task>(Paths.Yolo5ModelPath)
            .Build();

        var yolo5Model = neuralHub.GetModels<Yolo5Model>().First();
        
        // Act
        var result = await yolo5Model
            .RunAsync(DataProvider.LoadImageToBoxPredictionsTask(Paths.ValidImages));
        
        // Assert
        result.Should().NotBeNull();
    }
    
    [Fact]
    public async void GivenImageWithObjects_WhenRunAsync_ThenShouldReturnBoxPredictions()
    {
        // Arrange
        var neuralHub = NeuralHubConfiguration
            .FromDefaults()
            .AddYolo5Model(Paths.Yolo5ModelPath)
            .Build();

        var yolo5Model = neuralHub.GetModels<Yolo5Model>().First();
        
        // Act
        var result = await yolo5Model
            .RunAsync(DataProvider.LoadImageToBoxPredictionsTask(Paths.ValidImagesWithObjectsPaths));
        
        // Assert
        result.Should().NotBeNull();
    }
    
    [Fact]
    public async void GivenCluster_WhenRunAsync_ThenShouldReturnBoxPredictions()
    {
        // Arrange
        var modelCount = FakeData.IntFromSmallRange();
        
        var neuralHub = NeuralHubConfiguration
            .FromDefaults()
            .AddYolo5Models(Paths.Yolo5ModelPath, modelCount)
            .Build();

        var cluster = neuralHub.ShapeCluster<Cluster<Yolo5Model, ImageToBoxPredictionsYolo5Task>>();
        
        var task = new ImageToBoxPredictionsYolo5Task(DataProvider.LoadRandomImage(Paths.ValidImages));
        
        // Act
        var result = await cluster!.RunAsync(task);
        
        // Assert
        result.Should().NotBeNull();
    }
}
using FluentAssertions;
using Neural.Defaults;
using Neural.Defaults.Models;
using Neural.Onnx.Common;
using Neural.Onnx.Common.Options;
using Neural.Onnx.Models.ImageBoxPainter;
using Neural.Onnx.Models.Yolo5;
using Neural.Onnx.Models.Yolo5.Tasks.BoxPredictionsToImage;
using Neural.Onnx.Models.Yolo5.Tasks.ImageToBoxPredictions;
using Neural.Onnx.Pipelines.Yolo5;
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
            .AddOnnxModel<Yolo5Model, ImageToBoxPredictionsTask>(Paths.Yolo5ModelPath)
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

        var cluster = neuralHub.ShapeCluster<Cluster<Yolo5Model, ImageToBoxPredictionsTask>>();
        
        var task = new ImageToBoxPredictionsTask(DataProvider.LoadRandomImage(Paths.ValidImages));
        
        // Act
        var result = await cluster!.RunAsync(task);
        
        // Assert
        result.Should().NotBeNull();
    }
    
    [Fact]
    public async void GivenPredictions_WhenDrawPredictions_ThenShouldReturnValidImage()
    {
        // Arrange
        var modelCount = FakeData.IntFromSmallRange();
        
        var neuralHub = NeuralHubConfiguration
            .FromDefaults()
            .AddYolo5Models(Paths.Yolo5ModelPath, modelCount)
            .AddImageBoxPainterModels(new ImageBoxPainterOptions(), modelCount)
            .Build();

        var yolo5 = neuralHub.GetModels<Yolo5Model>().First();
        
        var imageBoxPainter = neuralHub.GetModels<ImageBoxPainterModel>().First();
        
        var task = new ImageToBoxPredictionsTask(DataProvider.LoadRandomImage(Paths.ValidImages));
        
        // Act
        var result = await yolo5.RunAsync(task);
        
        var imageTask = await imageBoxPainter.RunAsync(new BoxPredictionsToImageTask(result));
        
        // Assert
        result.Should().NotBeNull();
        imageTask.Should().NotBeNull();
    }
    
    [Fact]
    public async void GivenPlainPipeline_WhenProcessingImages_ThenShouldReturnValidImages()
    {
        // Arrange
        var modelCount = FakeData.IntFromSmallRange();
        
        var neuralHub = NeuralHubConfiguration
            .FromDefaults()
            .AddYolo5Models(Paths.Yolo5ModelPath, modelCount)
            .AddImageBoxPainterModels(new ImageBoxPainterOptions(), modelCount)
            .Build();

        var yolo5Pipeline = neuralHub.ExtractPipeline<Yolo5ImagePlainPipeline>();
        
        var inputImage = DataProvider.LoadImageToBoxPredictionsTask(Paths.ValidImages);
        
        // Act
        var result = await yolo5Pipeline!.RunTransparentAsync(inputImage);
        
        // Assert
        result.Should().NotBeNull();
        result!.TypedOutput.Image.Should().NotBeNull();
    }
}
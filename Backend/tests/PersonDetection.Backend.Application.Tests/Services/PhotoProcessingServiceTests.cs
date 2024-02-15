using FluentAssertions;
using Microsoft.Extensions.Options;
using Moq;
using PersonDetection.Backend.Application.Common.Exceptions;
using PersonDetection.Backend.Application.Services;
using PersonDetection.Backend.Application.Services.Implementations;
using PersonDetection.ImageProcessing.Options;
using PersonDetection.ImageSegmentation.ModelConverter;
using SixLabors.ImageSharp;

namespace PersonDetection.Backend.Application.Tests.Services;

public class PhotoProcessingServiceTests
{
    private readonly Mock<IOptions<YoloImageProcessingOptions>> _imageProcessingOptions;
    private readonly YoloImageSegmentation _yoloImageProcessing;
    private readonly ModelTypeProvider _modelTypeProvider;
    
    public PhotoProcessingServiceTests()
    {
        _imageProcessingOptions = new Mock<IOptions<YoloImageProcessingOptions>>();
        _imageProcessingOptions.Setup(options => options.Value).Returns(new YoloImageProcessingOptions
        {
            WeightsDirectory = "TestData/Weights/",
            WeightFile = "yolov5n",
            FontSize = 12
        });

        Mock<IOptions<ModelLoaderOptions>> modelLoaderOptions = new();
        modelLoaderOptions.Setup(options => options.Value).Returns(new ModelLoaderOptions
        {
            UnQuantizedModelPath = "TestData/Weights/yolov8n-seg-unquantize.onnx",
            QuantizedModelPath = "TestData/Weights/yolov8n-seg-quantize.onnx"
        });
        
        _yoloImageProcessing = new YoloImageSegmentation(modelLoaderOptions.Object);
        _modelTypeProvider = new ModelTypeProvider();
    }

    [Theory]
    [InlineData("")]
    [InlineData(null)]
    [InlineData(" ")]
    [InlineData("this string is not a base64 image")]
    public async Task GivenInvalidPhoto_WhenProcessPhotoAsync_ThenThrowsInvalidPhotoExceptions(string photoContent)
    {
        // Arrange
        var service = new PhotoProcessingService(_yoloImageProcessing, new ModelTypeProvider());

        // Act
        var act = async () => await service.ProcessPhotoAsync(photoContent);
        
        // Assert
        await act.Should().ThrowAsync<InvalidPhotoException>();
    }
    
    [Theory]
    [InlineData("")]
    [InlineData(null)]
    [InlineData(" ")]
    [InlineData("this string is not a base64 image")]
    public async Task GivenInvalidPhoto_WhenProcessPhotoTransparentAsync_ThenThrowsInvalidPhotoExceptions(string photoContent)
    {
        // Arrange
        var service = new PhotoProcessingService(_yoloImageProcessing, _modelTypeProvider);

        // Act
        var act = async () => await service.ProcessPhotoTransparentAsync(photoContent);
        
        // Assert
        await act.Should().ThrowAsync<InvalidPhotoException>();
    }

    [Theory]
    [InlineData("TestData/Images/Invalid/1.jpg.broken.jpg")]
    [InlineData("TestData/Images/Invalid/2.jpg.broken.jpg")]
    [InlineData("TestData/Images/Invalid/3.jpg.broken.jpg")]
    [InlineData("TestData/Images/Invalid/4.jpg.broken.jpg")]
    [InlineData("TestData/Images/Invalid/5.jpg.broken.jpg")]
    [InlineData("TestData/Images/Invalid/6.jpg.broken.jpg")]
    [InlineData("TestData/Images/Invalid/7.jpg.broken.jpg")]
    [InlineData("TestData/Images/Invalid/8.jpg.broken.jpg")]
    [InlineData("TestData/Images/Invalid/9.jpg.broken.jpg")]
    [InlineData("TestData/Images/Invalid/10.jpg.broken.jpg")]
    public void GivenInvalidPhotoFile_WhenProcessPhotoAsync_ThenThrowsInvalidImageContentExceptions(string path)
    {
        // Arrange
        var image = Convert.ToBase64String(File.ReadAllBytes(path));
        var service = new PhotoProcessingService(_yoloImageProcessing, _modelTypeProvider);

        // Act
        var act = async () => await service.ProcessPhotoAsync(image);
        
        // Assert
        act.Should().ThrowAsync<InvalidImageContentException>();
    }
    
    [Theory]
    [InlineData("TestData/Images/Invalid/1.jpg.broken.jpg")]
    [InlineData("TestData/Images/Invalid/2.jpg.broken.jpg")]
    [InlineData("TestData/Images/Invalid/3.jpg.broken.jpg")]
    [InlineData("TestData/Images/Invalid/4.jpg.broken.jpg")]
    [InlineData("TestData/Images/Invalid/5.jpg.broken.jpg")]
    [InlineData("TestData/Images/Invalid/6.jpg.broken.jpg")]
    [InlineData("TestData/Images/Invalid/7.jpg.broken.jpg")]
    [InlineData("TestData/Images/Invalid/8.jpg.broken.jpg")]
    [InlineData("TestData/Images/Invalid/9.jpg.broken.jpg")]
    [InlineData("TestData/Images/Invalid/10.jpg.broken.jpg")]
    public void GivenInvalidPhotoFile_WhenProcessPhotoTransparentAsync_ThenThrowsInvalidImageContentExceptions(string path)
    {
        // Arrange
        var image = Convert.ToBase64String(File.ReadAllBytes(path));
        var service = new PhotoProcessingService(_yoloImageProcessing, _modelTypeProvider);

        // Act
        var act = async () => await service.ProcessPhotoTransparentAsync(image);
        
        // Assert
        act.Should().ThrowAsync<InvalidImageContentException>();
    }
    
    [Theory]
    [InlineData("no_exist", "no_exist", "TestData/Images/Invalid/6.jpg.broken.jpg")]
    public void GivenNonExistingModelPath_WhenProcessPhotoAsync_ThenThrowsFileNotFoundException(string path, string file, string imagePath)
    {
        // Arrange
        var image = Convert.ToBase64String(File.ReadAllBytes(imagePath));
        _imageProcessingOptions.Setup(options => options.Value).Returns(new YoloImageProcessingOptions
        {
            WeightsDirectory = path,
            WeightFile = file,
            FontSize = 12
        });
        
        var service = new PhotoProcessingService(_yoloImageProcessing, _modelTypeProvider);
        
        // Act
        var act = async () => await service.ProcessPhotoAsync(image);
        
        // Assert
        act.Should().ThrowAsync<FileNotFoundException>();
    }
    
    [Theory]
    [InlineData("TestData/Images/Valid/1.jpg")]
    [InlineData("TestData/Images/Valid/2.jpg")]
    [InlineData("TestData/Images/Valid/3.jpg")]
    [InlineData("TestData/Images/Valid/4.jpg")]
    [InlineData("TestData/Images/Valid/5.jpg")]
    [InlineData("TestData/Images/Valid/6.jpg")]
    [InlineData("TestData/Images/Valid/7.jpg")]
    [InlineData("TestData/Images/Valid/8.jpg")]
    [InlineData("TestData/Images/Valid/9.jpg")]
    [InlineData("TestData/Images/Valid/10.jpg")]
    public async void GivenValidPhotoFile_WhenProcessPhotoAsync_ThenReturnsProcessedPhoto(string path)
    {
        // Arrange
        var image = Convert.ToBase64String(await File.ReadAllBytesAsync(path));
        var service = new PhotoProcessingService(_yoloImageProcessing, _modelTypeProvider);
        
        // Act
        var act = async () => await service.ProcessPhotoAsync(image);
        
        // Assert
        await act.Should().NotThrowAsync();
    }
    
    [Theory]
    [InlineData("TestData/Images/Valid/1.jpg")]
    [InlineData("TestData/Images/Valid/2.jpg")]
    [InlineData("TestData/Images/Valid/3.jpg")]
    [InlineData("TestData/Images/Valid/4.jpg")]
    [InlineData("TestData/Images/Valid/5.jpg")]
    [InlineData("TestData/Images/Valid/6.jpg")]
    [InlineData("TestData/Images/Valid/7.jpg")]
    [InlineData("TestData/Images/Valid/8.jpg")]
    [InlineData("TestData/Images/Valid/9.jpg")]
    [InlineData("TestData/Images/Valid/10.jpg")]
    public async void GivenValidPhotoFile_WhenProcessPhotoAsync_ThenReturnsValidPhotoContent(string path)
    {
        // Arrange
        var image = Convert.ToBase64String(await File.ReadAllBytesAsync(path));
        var service = new PhotoProcessingService(_yoloImageProcessing, _modelTypeProvider);
        var processedPhoto = await service.ProcessPhotoAsync(image);
        
        // Act
        var act = async () => await service.ProcessPhotoAsync(processedPhoto);
        
        // Assert
        act
            .Should().NotThrowAsync()
            .Should().NotBeNull();
    }
}

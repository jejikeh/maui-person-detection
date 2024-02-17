using FluentAssertions;
using Neural.Defaults;
using Neural.Tests.Common.Mocks.Models.Tasks;
using Neural.Tests.Common.Mocks.Models.Yolo5;
using Neural.Tests.Common.Mocks.Models.Yolo8;
using Neural.Tests.Common.Mocks.Options;

namespace Neural.Core.Tests;

public class NeuralHubTests
{
    [Fact]
    public void GivenModel_WhenGetModel_ThenReturnModel()
    {
        // Arrange
        var neuralHub = NeuralHubConfiguration
            .FromDefaults()
            .AddModel<Yolo5ModelMock, StringToStringTaskMock>(Yolo5Options.ModelPath)
            .Build();
        
        // Act
        var model = neuralHub.GetModels<Yolo5ModelMock>().First();
        
        // Assert
        model.Should().BeOfType<Yolo5ModelMock>();
    }

    [Fact]
    public void GivenModels_WhenGetModel_ThenReturnAllModel()
    {
        // Arrange
        var neuralHub = NeuralHubConfiguration
            .FromDefaults()
            .AddModel<Yolo5ModelMock, StringToStringTaskMock>(Yolo5Options.ModelPath)
            .AddModel<Yolo5ModelIntToStringMock, IntToStringTaskMock>(Yolo5Options.ModelPath)
            .AddModel<Yolo8ModelWithOptionsMock, StringToStringTaskMock, Yolo8QuantizedOptions>(new Yolo8QuantizedOptions())
            .Build();
        
        // Act
        var models = neuralHub.GetModels<Yolo5ModelMock>();
        
        // Assert
        models.Should().HaveCount(1);
    }

    [Fact]
    public void GivenModelTask_When()
    {
        // Arrange
        var neuralHub = NeuralHubConfiguration
            .FromDefaults()
            .AddModel<Yolo5ModelMock, StringToStringTaskMock>(Yolo5Options.ModelPath)
            // .AddPipeline(new ImageProcessPipeline())
            .Build();
        
        // Act
        
    }
}

// public class ImageProcessPipeline : IPipeline<string, string>
// {
//     public List<IModel> Cluster { get; }
//
//     public Task<string> RunAsync(string input)
//     {
//         throw new NotImplementedException();
//     }
// }
//
// public interface IPipeline<TInput, TOutput>
// {
//     Task<TOutput> RunAsync(TInput input);
// }
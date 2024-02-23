using Neural.Core.Models;
using Neural.Core.Models.Events;
using Neural.Onnx.Common.Dependencies;
using Neural.Onnx.Services;
using Neural.Onnx.Tasks.BoxPredictionsToImage;

namespace Neural.Onnx.Models.ImageBoxPainter;

public class ImageBoxPainterModel : IModel<BoxPredictionsToImageTasks, ImageBoxPainterDependencies>
{
    private IImageBoxPainterService? _imageBoxPainterService; 
    private ModelStatus _status = ModelStatus.Inactive;

    public ModelStatus Status
    {
        get => _status;
        set
        {
            _status = value;
            StatusChanged?.Invoke(this, new ModelStatusChangedEventArgs(value));
        }
    }
    
    public ImageBoxPainterDependencies? DependencyContainer { get; set; }

    public string Name { get; set; } = nameof(ImageBoxPainterModel);
    public event EventHandler<ModelStatusChangedEventArgs>? StatusChanged;
    
    void IModel<BoxPredictionsToImageTasks, ImageBoxPainterDependencies>.Initialize(
        ImageBoxPainterDependencies imageBoxPainterDependencies)
    {
        DependencyContainer = imageBoxPainterDependencies;
        
        _imageBoxPainterService = DependencyContainer.ImageBoxPainterService;
    }
    
    public Task<BoxPredictionsToImageTasks> RunAsync(BoxPredictionsToImageTasks task)
    {
        if (DependencyContainer is null)
        {
            throw new NullReferenceException(nameof(DependencyContainer));
        }
        
        Status = ModelStatus.Active;

        var inputImage = task.BoxPredictionsInput().InputImage;
        var predictions = task.BoxPredictionsInput().Predictions;
        
        _imageBoxPainterService!.PaintPredictions(inputImage, predictions);
        
        task.SetOutput(this, inputImage);
        
        Status = ModelStatus.Inactive;
        
        return Task.FromResult(task);
    }

    public BoxPredictionsToImageTasks TryRunInBackground(BoxPredictionsToImageTasks input)
    {
        Status = ModelStatus.Active;

        Task.Run(async () => await RunAsync(input));

        return input;
    }
}
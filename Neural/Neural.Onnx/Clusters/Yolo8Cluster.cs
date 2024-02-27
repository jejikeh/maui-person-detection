using Neural.Core.Models;
using Neural.Defaults.Models;
using Neural.Onnx.Models.Yolo5;
using Neural.Onnx.Models.Yolo5.Tasks.ImageToBoxPredictions;
using Neural.Onnx.Models.Yolo8;
using Neural.Onnx.Tasks.ImageToSegmentation;

namespace Neural.Onnx.Clusters;

public class Yolo8Cluster : Cluster<Yolo8Model, ImageToSegmentationTask>
{
    public async void RunInBackground(string input, Func<ImageToSegmentationTask, Task> handleModelCompleteAsync)
    {
        var model = await WaitUntilModelWithStatusAsync(ModelStatus.Inactive);
        
        var modelTask = model?.TryRunInBackground(new ImageToSegmentationTask(input));
        
        if (modelTask is null)
        {
            return;
        }
        
        modelTask.OnModelTaskComplete += completedTask =>
        {
            if (completedTask is not ImageToSegmentationTask imageToSegmentationTask)
            {
                return;
            }
            
            handleModelCompleteAsync(imageToSegmentationTask);
        };
    }
}
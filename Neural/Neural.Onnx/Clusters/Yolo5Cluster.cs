using Neural.Core.Models;
using Neural.Defaults.Models;
using Neural.Onnx.Models.Yolo5;
using Neural.Onnx.Models.Yolo5.Tasks.ImageToBoxPredictions;

namespace Neural.Onnx.Clusters;

public class Yolo5Cluster : Cluster<Yolo5Model, ImageToBoxPredictionsTask>
{
    public void RunInBackground(string input, Func<ImageToBoxPredictionsTask, Task> handleModelCompleteAsync)
    {
        var model = GetModelWithStatus(ModelStatus.Inactive);
        
        var modelTask = model?.TryRunInBackground(new ImageToBoxPredictionsTask(input));
        
        if (modelTask is null)
        {
            return;
        }
        
        modelTask.OnModelTaskComplete += completedTask =>
        {
            if (completedTask is not ImageToBoxPredictionsTask imageToBoxCompletedTask)
            {
                return;
            }
            
            handleModelCompleteAsync(imageToBoxCompletedTask);
        };
    }
}
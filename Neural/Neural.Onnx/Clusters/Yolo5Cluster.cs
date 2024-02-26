using System.Collections;
using System.Collections.Concurrent;
using Neural.Core.Models;
using Neural.Defaults.Models;
using Neural.Onnx.Models.Yolo5;
using Neural.Onnx.Models.Yolo5.Tasks.ImageToBoxPredictions;

namespace Neural.Onnx.Clusters;

public class Yolo5Cluster : Cluster<Yolo5Model, ImageToBoxPredictionsTask>
{
    public async Task RunAsync(IAsyncEnumerable<string> inputs, Func<ImageToBoxPredictionsTask, Task> handleModelCompleted)
    {
        await foreach (var input in inputs)
        {
            var model = await WaitUntilModelWithStatusAsync(ModelStatus.Inactive);

            if (model is null)
            {
                continue;
            }

            await handleModelCompleted(new ImageToBoxPredictionsTask(input));
        }
    }
    
    // public async IAsyncEnumerable<ImageToBoxPredictionsTask?> RunAsync(ConcurrentQueue<string> input)
    // {
    //     foreach (var inputTask in input)
    //     {
    //         var model = await WaitUntilModelWithStatusAsync(ModelStatus.Inactive);
    //
    //         if (model is null)
    //         {
    //             yield break;
    //         }
    //
    //         yield return await model.RunAsync(new ImageToBoxPredictionsTask(inputTask));
    //     }
    // }


    public async IAsyncEnumerable<ConcurrentQueue<ImageToBoxPredictionsTask>> RunAsync(ConcurrentQueue<string> input)
    {
        var output = new ConcurrentQueue<ImageToBoxPredictionsTask>();
    
        await Parallel.ForEachAsync(Models, async (model, _) =>
        {
            var buffer = input.TryDequeue(out var task);
    
            if (!buffer || string.IsNullOrEmpty(task))
            {
                return;
            }
            
            var modelTask = await model.RunAsync(new ImageToBoxPredictionsTask(task));
            
            output.Enqueue(modelTask);
        });
    
        yield return output;
    }
}
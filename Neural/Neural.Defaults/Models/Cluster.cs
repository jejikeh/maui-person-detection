using Neural.Core;
using Neural.Core.Models;
using Neural.Core.Models.Events;

namespace Neural.Defaults.Models;

public class Cluster<TModel, TModelTask> : ICluster<TModel, TModelTask> 
    where TModel : class, IModel<TModelTask> 
    where TModelTask : class, IModelTask
{
    protected readonly List<TModel> Models = [];
    
    public bool Init(NeuralHub neuralHub)
    {
        var models = neuralHub.GetModels<TModel>().ToArray();

        if (models.Length == 0)
        {
            return false;
        }
        
        Models.AddRange(models);

        return true;
    }

    public TModel? GetModelWithStatus(ModelStatus status)
    {
        return Models.FirstOrDefault(model => model.Status == status);
    }
    
    public async Task RunHandleAsync(TModelTask input, Func<TModelTask, Task> handleModelCompleted)
    {
        var output = await RunAsync(input);

        if (output is not null)
        {
            await handleModelCompleted(output);
        }
    }

    protected async Task<TModel?> WaitUntilModelWithStatusAsync(ModelStatus status)
    {
        if (Models.Count == 0)
        {
            return null;
        }
        
        var modelWithStatus = GetModelWithStatus(status);

        if (modelWithStatus is not null)
        {
            return modelWithStatus;
        }
        
        var taskCompletionSource = new TaskCompletionSource<TModel>();

        EventHandler<ModelStatusChangedEventArgs> handler = null!;
        handler = (sender, e) =>
        {
            if (e.NewStatus != status)
            {
                return;
            }
            
            var model = sender as TModel ?? throw new InvalidOperationException();
            model.StatusChanged -= handler;

            if (!taskCompletionSource.Task.IsCompleted)
            {
                taskCompletionSource.SetResult(model);
            }
        };

        foreach (var model in Models)
        {
            model.StatusChanged += handler;
        }

        return await taskCompletionSource.Task;
    }
    
    public async Task RunHandleAsync(IEnumerable<TModelTask> inputs, Func<TModelTask, Task> handleModelCompleted)
    {
        await Parallel.ForEachAsync(inputs, async (input, _) =>
        {
            var output = await RunAsync(input);

            if (output is not null)
            {
                await handleModelCompleted(output);
            }
        });
    }

    public async Task<TModelTask?> RunAsync(TModelTask input)
    {
        var model = GetModelWithStatus(ModelStatus.Inactive);
        
        if (model is null)
        {
            return null;
        }
        
        return await model.RunAsync(input);
    }
    
    public async IAsyncEnumerable<TModelTask?> RunAsync(IAsyncEnumerable<TModelTask> input)
    {
        await foreach (var inputTask in input)
        {
            var model = await WaitUntilModelWithStatusAsync(ModelStatus.Inactive);

            if (model is null)
            {
                continue;
            }

            yield return await model.RunAsync(inputTask);
        }
    }

    public int Count()
    {
        return Models.Count;
    }

    public async Task<TModelTask?> RunInBackgroundAsync(TModelTask input)
    {
        var model = await WaitUntilModelWithStatusAsync(ModelStatus.Inactive);

        var modelTask = model?.TryRunInBackground(input);

        return modelTask;
    }
    
    public bool IsAnyModelWithStatus(ModelStatus status)
    {
        return GetModelWithStatus(status) is not null;
    }
}
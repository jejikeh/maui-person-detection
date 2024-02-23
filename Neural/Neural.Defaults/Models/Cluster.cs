using Neural.Core;
using Neural.Core.Models;
using Neural.Core.Models.Events;

namespace Neural.Defaults.Models;

public class Cluster<TModel, TModelTask> : ICluster<TModel, TModelTask> 
    where TModel : class, IModel<TModelTask> 
    where TModelTask : class, IModelTask
{
    private readonly List<TModel> _models = [];
    
    public bool Init(NeuralHub neuralHub)
    {
        var models = neuralHub.GetModels<TModel>().ToArray();

        if (models.Length == 0)
        {
            return false;
        }
        
        _models.AddRange(models);

        return true;
    }

    public TModel? GetModelWithStatus(ModelStatus status)
    {
        return _models.FirstOrDefault(model => model.Status == status);
    }
    
    public async Task RunHandleAsync(TModelTask input, Func<TModelTask, Task> handleModelCompleted)
    {
        var output = await RunAsync(input);

        if (output is not null)
        {
            await handleModelCompleted(output);
        }
    }

    private async Task<TModel?> WaitUntilModelWithStatus(ModelStatus status)
    {
        if (_models.Count == 0)
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

        foreach (var model in _models)
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
        var model = await WaitUntilModelWithStatus(ModelStatus.Inactive);

        if (model is null)
        {
            return null;
        }
        
        return await model.RunAsync(input);
    }

    public int Count()
    {
        return _models.Count;
    }

    public async Task<TModelTask?> RunInBackgroundAsync(TModelTask input)
    {
        var model = await WaitUntilModelWithStatus(ModelStatus.Inactive);

        var modelTask = model?.TryRunInBackground(input);

        return modelTask;
    }
    
    public bool IsAnyModelWithStatus(ModelStatus status)
    {
        return GetModelWithStatus(status) is not null;
    }
}
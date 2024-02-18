using Neural.Core.Models;

namespace Neural.Core;

public class NeuralHub
{
    public List<IModel> Models { get; } = new List<IModel>();

    public IEnumerable<TModel> GetModels<TModel>()
    {
        return Models
            .Where(model => model.GetType() == typeof(TModel))
            .Cast<TModel>();
    }

    public async Task<TModelTask?> RunAsync<TModel, TModelTask>(TModelTask input) 
        where TModel : class, IModel<TModelTask>
        where TModelTask : IModelTask
    {
        var model = GetModels<TModel>().FirstOrDefault();
        
        if (model is null)
        {
            return default;
        }

        return await model.RunAsync(input);
    }

    public async Task<TModelTask?> RunAsync<TModelTask>(TModelTask input) 
        where TModelTask : IModelTask
    {
        if (Models.FirstOrDefault(rawModel => rawModel.CanProcess(input)) is not IModel<TModelTask> model)
        {
            return default;
        }
        
        return await model.RunAsync(input);
    }
    
    public TModelTask? TryRunInBackground<TModelTask>(TModelTask input) 
        where TModelTask : IModelTask
    {
        if (Models.FirstOrDefault(rawModel => rawModel.CanProcess(input)) is not IModel<TModelTask> model)
        {
            return default;
        }
        
        return model.TryRunInBackground(input);
    }
}
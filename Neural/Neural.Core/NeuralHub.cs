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
}
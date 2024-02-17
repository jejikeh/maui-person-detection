using Neural.Core.Models;

namespace Neural.Core;

public class NeuralHub
{
    public List<IModel> Models { get; } = new List<IModel>();
}
using Neural.Core.Models;

namespace Neural.Core.Tests.Mocks.Models;

public class ClusterMock<TModel> : ICluster<TModel> where TModel : IModel
{
    private readonly List<TModel> _models = [];
    
    public void AddRange(IEnumerable<TModel> models)
    {
        _models.AddRange(models);
    }

    public TModel? GetModel(ModelStatus status)
    {
        return _models.FirstOrDefault(model => model.Status == status);
    }

    public int Count()
    {
        return _models.Count;
    }
}
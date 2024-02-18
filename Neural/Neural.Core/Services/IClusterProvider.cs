using Neural.Core.Models;

namespace Neural.Core.Services;

public interface IClusterProvider
{
    public ICluster<TModel> GetCluster<TModel>(IEnumerable<TModel> models) where TModel : IModel;
}
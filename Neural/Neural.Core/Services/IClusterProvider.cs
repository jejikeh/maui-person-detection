using Neural.Core.Models;

namespace Neural.Core.Services;

public interface IClusterProvider
{
    public ICluster<TModel, TModelTask> GetCluster<TModel, TModelTask>(IEnumerable<TModel> models)
        where TModel : class, IModel<TModelTask>
        where TModelTask : class, IModelTask;
}
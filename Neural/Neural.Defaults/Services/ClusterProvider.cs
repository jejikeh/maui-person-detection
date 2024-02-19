using Neural.Core.Models;
using Neural.Core.Services;
using Neural.Defaults.Models;

namespace Neural.Defaults.Services;

public class ClusterProvider : IClusterProvider
{
    public ICluster<TModel, TModelTask> GetCluster<TModel, TModelTask>(IEnumerable<TModel> models) 
        where TModel : class, IModel<TModelTask> 
        where TModelTask : class, IModelTask
    {
        var cluster = new Cluster<TModel, TModelTask>();
        
        cluster.AddRange(models);
        
        return cluster;
    }
}
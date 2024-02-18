using Neural.Core.Models;
using Neural.Core.Services;
using Neural.Defaults.Models;

namespace Neural.Defaults.Services;

public class ClusterProvider : IClusterProvider
{
    public ICluster<TModel> GetCluster<TModel>(IEnumerable<TModel> models) where TModel : IModel
    {
        var cluster = new Cluster<TModel>();
        
        cluster.AddRange(models);
        
        return cluster;
    }
}
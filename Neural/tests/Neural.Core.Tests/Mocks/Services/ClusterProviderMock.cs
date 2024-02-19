using Neural.Core.Models;
using Neural.Core.Services;
using Neural.Core.Tests.Mocks.Models;

namespace Neural.Core.Tests.Mocks.Services;

public class ClusterProviderMock : IClusterProvider
{
    public ICluster<TModel, TModelTask> GetCluster<TModel, TModelTask>(IEnumerable<TModel> models) 
        where TModel : IModel<TModelTask> 
        where TModelTask : class, IModelTask
    {
        var cluster = new ClusterMock<TModel, TModelTask>();
        
        cluster.AddRange(models);
        
        return cluster;
    }
}
using Neural.Core.Models;
using Neural.Core.Services;
using Neural.Core.Tests.Mocks.Models;

namespace Neural.Core.Tests.Mocks.Services;

public class ClusterProviderMock : IClusterProvider
{
    public ICluster<TModel> GetCluster<TModel>(IEnumerable<TModel> models) where TModel : IModel
    {
        var cluster = new ClusterMock<TModel>();
        
        cluster.AddRange(models);
        
        return cluster;
    }
}
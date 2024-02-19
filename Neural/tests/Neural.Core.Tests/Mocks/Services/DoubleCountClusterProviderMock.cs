using Neural.Core.Models;
using Neural.Core.Services;
using Neural.Core.Tests.Mocks.Models;

namespace Neural.Core.Tests.Mocks.Services;

public class DoubleCountClusterProviderMock : IClusterProvider
{
    public ICluster<TModel, TModelTask> GetCluster<TModel, TModelTask>(IEnumerable<TModel> models) 
        where TModel : IModel<TModelTask> 
        where TModelTask : class, IModelTask
    {
        var cluster = new ClusterMock<TModel, TModelTask>();
        
        var modelsArray = models.ToArray();
        
        cluster.AddRange(modelsArray);
        
        cluster.AddRange(modelsArray);
        
        return cluster;
    }
}
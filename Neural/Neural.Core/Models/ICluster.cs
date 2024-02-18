namespace Neural.Core.Models;

public interface ICluster<TModel> where TModel : IModel 
{
    public void AddRange(IEnumerable<TModel> models);
    public TModel? GetModel(ModelStatus status);
    public int Count();
}
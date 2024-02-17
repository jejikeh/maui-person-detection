namespace Neural.Core.Models;

public interface IModelTask
{
    public IModelInput Input { get; }
    public IModelOutput Output { get; set; }
}
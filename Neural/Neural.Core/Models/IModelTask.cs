namespace Neural.Core.Models;

public interface IModelTask
{
    public IModelInput Input { get; set; }
    public IModelOutput Output { get; set; }
    public void SetOutput(IModel model, object value);
}
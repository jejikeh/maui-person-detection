namespace Neural.Core.Models;

public class ModelStatusChangedEventArgs(ModelStatus _modelStatus) : EventArgs
{
    public ModelStatus NewStatus { get; } = _modelStatus;
}
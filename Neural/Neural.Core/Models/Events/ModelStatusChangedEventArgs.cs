namespace Neural.Core.Models.Events;

public class ModelStatusChangedEventArgs(ModelStatus _modelStatus) : EventArgs
{
    public ModelStatus NewStatus { get; } = _modelStatus;
}
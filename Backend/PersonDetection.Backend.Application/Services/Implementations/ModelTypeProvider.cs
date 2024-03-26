using PersonDetection.Backend.Application.Common.Models;

namespace PersonDetection.Backend.Application.Services.Implementations;

public class ModelTypeProvider
{
    public OnnxModelType ModelType { get; private set; }

    public OnnxModelType SwitchModelType()
    {
        ModelType = ModelType == OnnxModelType.Yolo5 
            ? OnnxModelType.Yolo8 
            : OnnxModelType.Yolo5;

        return ModelType;
    }
}
using PersonDetection.ImageSegmentation.ModelConverter;

namespace PersonDetection.Backend.Application.Services.Implementations;

public class ModelTypeProvider
{
    public ModelType ModelType { get; private set; }

    public ModelType SwitchModelType()
    {
        return ModelType = ModelType == ModelType.UnQuantized ? ModelType.Quantized : ModelType.UnQuantized;
    }
}
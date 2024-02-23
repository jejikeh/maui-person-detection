using Microsoft.ML.OnnxRuntime.Tensors;
using Neural.Onnx.Models.Yolo5.Specifications;

namespace Neural.Onnx.Models.Yolo8.Specifications;

public static class Yolo8TensorSpecification
{
    public static int BatchSize { get; set; } = 1;

    public static int ColorChannels { get; set; } = 3;
    
    public static DenseTensor<float> Tensor()
    {
        return new DenseTensor<float>([
            BatchSize,
            ColorChannels, 
            Yolo5Specification.InputSize.Height, 
            Yolo5Specification.InputSize.Width
        ]);
    }
}
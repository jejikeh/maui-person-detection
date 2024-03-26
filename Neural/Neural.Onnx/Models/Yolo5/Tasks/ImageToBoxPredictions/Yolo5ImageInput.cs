using Microsoft.ML.OnnxRuntime;
using Microsoft.ML.OnnxRuntime.Tensors;
using Neural.Core.Models;
using Neural.Onnx.Common;
using Neural.Onnx.Models.Yolo5.Specifications;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;

namespace Neural.Onnx.Models.Yolo5.Tasks.ImageToBoxPredictions;

public class Yolo5ImageInput(Image<Rgba32> _image) : IModelInput
{
    public readonly Image<Rgba32> Image = _image.ResizeImage(Yolo5Specification.InputSize);
    
    public List<NamedOnnxValue> GetNamedOnnxValues()
    {
        return [NamedOnnxValue.CreateFromTensor(Yolo5InputSpecification.TensorName, ToTensor())];
    }

    private Tensor<float> ToTensor()
    {
        var tensor = Yolo5TensorSpecification.Tensor();

        var chunkSize = Environment.ProcessorCount;
        var chunkCount = CalculateChunkCount(chunkSize);

        var tasks = Enumerable.Range(0, chunkCount).AsParallel().Select(async chunkIndex =>
        {
            var startRow = chunkIndex * chunkSize;
            var endRow = Math.Min(startRow + chunkSize, Yolo5Specification.InputSize.Height);

            for (var y = startRow; y < endRow; y++)
            {
                for (var x = 0; x < Yolo5Specification.InputSize.Width; x++)
                {
                    tensor.FillTensorFromRgbImage(Image, x, y);
                }
            }
        });

        Task.WaitAll(tasks.ToArray());

        return tensor;
    }

    private static int CalculateChunkCount(int chunkSize)
    {
        return (int)Math.Ceiling((double)Yolo5Specification.InputSize.Height / chunkSize);
    }
}
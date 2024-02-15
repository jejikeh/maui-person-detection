using Microsoft.ML.OnnxRuntime;
using Microsoft.ML.OnnxRuntime.Tensors;
using PersonDetection.ImageSegmentation.Model.Data.Input;
using PersonDetection.ImageSegmentation.Model.Data.Output;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;

namespace PersonDetection.ImageSegmentation.Model;

public class Instance
{
    private readonly InferenceSession _inferenceSession;
    private readonly string[] _inputNames;

    public Instance(string modelPath)
    {
        _inferenceSession = new InferenceSession(File.ReadAllBytes(modelPath), new SessionOptions());
        _inputNames = _inferenceSession.InputMetadata.Keys.ToArray();
    }

    public Segmentation Predict(Image<Rgb24> image)
    {
        var originSize = image.Size;
        var input = image.ToTensor();
        var inputs = MapNamedOnnxValues([input]);
        using var results = _inferenceSession.Run(inputs);
        
        var modelOnnxOutput = new List<NamedOnnxValue>(results);
        
        var parser = new SegmentationOutputParser();
        var boxesOutput = modelOnnxOutput[0].AsTensor<float>();
        var maskPrototypes = modelOnnxOutput[1].AsTensor<float>();

        var boxes = SegmentationOutputParser.Parse(boxesOutput, maskPrototypes, originSize);

        return new Segmentation
        {
            Boxes = boxes,
        };
    }
    
    private NamedOnnxValue[] MapNamedOnnxValues(ReadOnlySpan<Tensor<float>> inputs)
    {
        var length = inputs.Length;
        var values = new NamedOnnxValue[length];

        for (var i = 0; i < length; i++)
        {
            var name = _inputNames[i];
            var value = NamedOnnxValue.CreateFromTensor(name, inputs[i]);
            values[i] = value;
        }

        return values;
    }
}
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Maui.Storage;
using Microsoft.ML.OnnxRuntime;
using Microsoft.ML.OnnxRuntime.Tensors;
using PersonDetection.ImageProcessing.Model.Extensions;
using PersonDetection.ImageProcessing.Model.Models;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;

namespace PersonDetection.ImageProcessing.Model;

// Original source code
// https://github.com/techwingslab/yolov5-net

public class YoloScorer : IDisposable
{
    private InferenceSession _inferenceSession;
    private readonly YoloCocoP5Model _model = new();
    
    public static async Task<YoloScorer> CreateAsync(Stream weightStream)
    {
        using var memoryStream = new MemoryStream();
        await weightStream.CopyToAsync(memoryStream);
        var bytes = memoryStream.ToArray();
        
        var yoloScorer = new YoloScorer();
        yoloScorer._inferenceSession = new InferenceSession(bytes, new SessionOptions());
        
        return yoloScorer;
    }

    public void Dispose()
    {
        _inferenceSession.Dispose();
    }

    private static float Sigmoid(float value)
    {
        return 1 / (1 + (float)Math.Exp(-value));
    }

    private float[] Xywh2xyxy(float[] source)
    {
        var result = new float[4];

        result[0] = source[0] - source[2] / 2f;
        result[1] = source[1] - source[3] / 2f;
        result[2] = source[0] + source[2] / 2f;
        result[3] = source[1] + source[3] / 2f;

        return result;
    }

    private float Clamp(float value, float min, float max)
    {
        return value < min ? min : value > max ? max : value;
    }

    private Tensor<float> ExtractPixels(Image<Rgba32> image)
    {
        var tensor = new DenseTensor<float>(new[] { 1, 3, _model.Height, _model.Width });

        Parallel.For(0, image.Height, y =>
        {
            Parallel.For(0, image.Width, x =>
            {
                tensor[0, 0, y, x] = image[x, y].R / 255.0F;
                tensor[0, 1, y, x] = image[x, y].G / 255.0F;
                tensor[0, 2, y, x] = image[x, y].B / 255.0F;
            });
        });

        return tensor;
    }

    private DenseTensor<float>[] Inference(Image<Rgba32> image)
    {
        if (image.Width != _model.Width || image.Height != _model.Height)
        {
            image.Mutate(x => x.Resize(_model.Width, _model.Height));
        }

        var inputs = new List<NamedOnnxValue> 
        {
            NamedOnnxValue.CreateFromTensor("images", ExtractPixels(image))
        };

        var result = _inferenceSession.Run(inputs);

        return _model.Outputs
            .Select(item => result.First(x => x.Name == item).Value as DenseTensor<float>)
            .ToArray();
    }

    private List<YoloPrediction> ParseDetect(DenseTensor<float> output, (int Width, int Height) image)
    {
        var result = new ConcurrentBag<YoloPrediction>();

        var xGain = _model.Width / (float)image.Width;
        var yGain = _model.Height / (float)image.Height;

        var xPadding = (_model.Width - image.Width * xGain) / 2;
        var yPadding = (_model.Height - image.Height * yGain) / 2;

        Parallel.For(0, (int)output.Length / _model.Dimensions, i =>
        {
            if (output[0, i, 4] <= _model.Confidence)
            {
                return;
            }

            Parallel.For(5, _model.Dimensions, j => output[0, i, j] *= output[0, i, 4]);
            
            Parallel.For(5, _model.Dimensions, k =>
            {
                if (output[0, i, k] <= _model.MulConfidence)
                {
                    return;
                }

                var xMin = (output[0, i, 0] - output[0, i, 2] / 2 - xPadding) / xGain; 
                var yMin = (output[0, i, 1] - output[0, i, 3] / 2 - yPadding) / yGain; 
                var xMax = (output[0, i, 0] + output[0, i, 2] / 2 - xPadding) / xGain; 
                var yMax = (output[0, i, 1] + output[0, i, 3] / 2 - yPadding) / yGain; 

                xMin = Clamp(xMin, 0, image.Width - 0); 
                yMin = Clamp(yMin, 0, image.Height - 0); 
                xMax = Clamp(xMax, 0, image.Width - 1); 
                yMax = Clamp(yMax, 0, image.Height - 1); 

                var label = _model.Labels[k - 5];

                var prediction = new YoloPrediction(
                    label, 
                    output[0, i, k],
                    new RectangleF(xMin, yMin, xMax - xMin, yMax - yMin));

                result.Add(prediction);
            });
        });

        return result.ToList();
    }

    private List<YoloPrediction> ParseSigmoid(DenseTensor<float>[] output, (int Width, int Height) image)
    {
        var result = new ConcurrentBag<YoloPrediction>();

        var xGain = _model.Width / (float)image.Width;
        var yGain = _model.Height / (float)image.Height;

        var xPadding  = (_model.Width - image.Width * xGain) / 2;
        var yPadding = (_model.Height - image.Height * yGain) / 2; 

        Parallel.For(0, output.Length, i => 
        {
            var shapes = _model.Shapes[i]; 
            Parallel.For(0, _model.Anchors[0].Length, a => 
            {
                Parallel.For(0, shapes, y => 
                {
                    Parallel.For(0, shapes, x => 
                    {
                        var offset = (shapes * shapes * a + shapes * y + x) * _model.Dimensions;
                        var buffer = output[i].Skip(offset).Take(_model.Dimensions).Select(Sigmoid).ToArray();

                        if (buffer[4] <= _model.Confidence)
                        {
                            return;
                        } 

                        var scores = buffer.Skip(5).Select(b => b * buffer[4]).ToList();

                        var mulConfidence = scores.Max();

                        if (mulConfidence <= _model.MulConfidence) 
                        {
                            return;
                        }

                        var rawX = (buffer[0] * 2 - 0.5f + x) * _model.Strides[i]; 
                        var rawY = (buffer[1] * 2 - 0.5f + y) * _model.Strides[i]; 

                        var rawW = (float)Math.Pow(buffer[2] * 2, 2) * _model.Anchors[i][a][0]; 
                        var rawH = (float)Math.Pow(buffer[3] * 2, 2) * _model.Anchors[i][a][1]; 

                        var xyxy = Xywh2xyxy(new[] { rawX, rawY, rawW, rawH });

                        var xMin = Clamp((xyxy[0] - xPadding) / xGain, 0, image.Width - 0); 
                        var yMin = Clamp((xyxy[1] - yPadding) / yGain, 0, image.Height - 0); 
                        var xMax = Clamp((xyxy[2] - xPadding) / xGain, 0, image.Width - 1); 
                        var yMax = Clamp((xyxy[3] - yPadding) / yGain, 0, image.Height - 1); 

                        var label = _model.Labels[scores.IndexOf(mulConfidence)];

                        var prediction = new YoloPrediction(
                            label, 
                            mulConfidence,
                            new RectangleF(xMin, yMin, xMax - xMin, yMax - yMin));

                        result.Add(prediction);
                    });
                });
            });
        });

        return result.ToList();
    }

    private List<YoloPrediction> ParseOutput(DenseTensor<float>[] output, (int Width, int Height) image)
    {
        return _model.UseDetect ? ParseDetect(output[0], image) : ParseSigmoid(output, image);
    }

    private List<YoloPrediction> Suppress(List<YoloPrediction> items)
    {
        var result = new List<YoloPrediction>(items);

        foreach (var item in items)
        {
            foreach (var current in result.ToList().Where(current => current != item))
            {
                var rect1 = item.Rectangle;
                var rect2 = current.Rectangle;

                var intersection = RectangleF.Intersect(rect1, rect2);

                var intArea = intersection.Area();
                var unionArea = rect1.Area() + rect2.Area() - intArea;
                var overlap = intArea / unionArea;

                if (!(overlap >= _model.Overlap))
                {
                    continue;
                }

                if (item.Score >= current.Score)
                {
                    result.Remove(current);
                }
            }
        }

        return result;
    }

    public List<YoloPrediction> Predict(Image<Rgba32> image)
    {
        return Suppress(ParseOutput(Inference(image.Clone()), (image.Width, image.Height)));
    }
}
using Compunet.YoloV8.Parsers;
using Microsoft.ML.OnnxRuntime.Tensors;
using SixLabors.ImageSharp;

namespace PersonDetection.ImageSegmentation.Model;

internal readonly struct IndexedBoundingBoxParser
{
    public IndexedBoundingBox[] Parse(Tensor<float> output, Size originSize)
    {
        var reductionRatio = Math.Min(YoloSegmentationOptions.Width / (float)originSize.Width, YoloSegmentationOptions.Height / (float)originSize.Height);
        var xPadding = (int)((YoloSegmentationOptions.Width - originSize.Width * reductionRatio) / 2);
        var yPadding = (int)((YoloSegmentationOptions.Height - originSize.Height * reductionRatio) / 2);

        return Parse(output, originSize, xPadding, yPadding);
    }

    public IndexedBoundingBox[] Parse(Tensor<float> output, Size originSize, int xPadding, int yPadding)
    {
        var xRatio = (float)originSize.Width / YoloSegmentationOptions.Width;
        var yRatio = (float)originSize.Height / YoloSegmentationOptions.Height;

        var maxRatio = Math.Max(xRatio, yRatio);

        xRatio = maxRatio;
        yRatio = maxRatio;

        return Parse(output, originSize, xPadding, yPadding, xRatio, yRatio);
    }

    private IndexedBoundingBox[] Parse(Tensor<float> output, Size originSize, int xPadding, int yPadding, float xRatio, float yRatio)
    {
        var boxes = new IndexedBoundingBox[output.Dimensions[2]];

        Parallel.For(0, output.Dimensions[2], i =>
        {
            for (int j = 0; j < YoloSegmentationOptions.Classes.Length; j++)
            {
                var confidence = output[0, j + 4, i];

                if (confidence <= YoloSegmentationOptions.ConfidenceThreshold)
                    continue;

                var x = output[0, 0, i];
                var y = output[0, 1, i];
                var w = output[0, 2, i];
                var h = output[0, 3, i];

                var xMin = (int)((x - w / 2 - xPadding) * xRatio);
                var yMin = (int)((y - h / 2 - yPadding) * yRatio);
                var xMax = (int)((x + w / 2 - xPadding) * xRatio);
                var yMax = (int)((y + h / 2 - yPadding) * yRatio);

                xMin = Math.Clamp(xMin, 0, originSize.Width);
                yMin = Math.Clamp(yMin, 0, originSize.Height);
                xMax = Math.Clamp(xMax, 0, originSize.Width);
                yMax = Math.Clamp(yMax, 0, originSize.Height);

                var name = YoloSegmentationOptions.Classes[j];
                var bounds = Rectangle.FromLTRB(xMin, yMin, xMax, yMax);

                boxes[i] = new IndexedBoundingBox
                {
                    Index = i,
                    Class = name,
                    Bounds = bounds,
                    Confidence = confidence
                };
            }
        });

        var count = boxes.Count(t => t.IsEmpty == false);

        var topBoxes = new IndexedBoundingBox[count];

        var topIndex = 0;

        foreach (var box in boxes)
        {
            if (box.IsEmpty)
            {
                continue;
            }

            topBoxes[topIndex++] = box;
        }

        return Suppress(topBoxes);
    }

    private static IndexedBoundingBox[] Suppress(IndexedBoundingBox[] boxes, float iouThreshold = 0.45f)
    {
        Array.Sort(boxes);

        var boxCount = boxes.Length;
        var activeCount = boxCount;
        var isNotActiveBoxes = new bool[boxCount];
        var selected = new List<IndexedBoundingBox>();

        for (var i = 0; i < boxCount; i++)
        {
            if (isNotActiveBoxes[i])
            {
                continue;
            }

            var boxA = boxes[i];

            selected.Add(boxA);

            for (var j = i + 1; j < boxCount; j++)
            {
                if (isNotActiveBoxes[j])
                {
                    continue;
                }

                var boxB = boxes[j];

                if (!(CalculateIoU(boxA.Bounds, boxB.Bounds) > iouThreshold))
                {
                    continue;
                }
                
                isNotActiveBoxes[j] = true;
                activeCount--;

                if (activeCount <= 0)
                {
                    break;
                }
            }

            if (activeCount <= 0)
            {
                break;
            }
        }

        return [.. selected];
    }

    private static float CalculateIoU(Rectangle rectA, Rectangle rectB)
    {
        var areaA = Area(rectA);

        if (areaA <= 0f)
        {
            return 0f;
        }

        var areaB = Area(rectB);

        if (areaB <= 0f)
        {
            return 0f;
        }

        var intersectionArea = Area(Rectangle.Intersect(rectA, rectB));

        return (float)intersectionArea / (areaA + areaB - intersectionArea);
    }

    private static int Area(Rectangle rectangle)
    {
        return rectangle.Width * rectangle.Height;
    }
}
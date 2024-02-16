using Microsoft.ML.OnnxRuntime.Tensors;
using PersonDetection.ImageSegmentation.Model.Data;
using PersonDetection.ImageSegmentation.Model.Data.Output;
using SixLabors.ImageSharp;
using Rectangle = PersonDetection.ImageSegmentation.Model.Data.Rectangle;

namespace PersonDetection.ImageSegmentation.Model;

internal readonly struct IndexedBoundingBoxParser
{
    public static IndexedBoundingBox[] Parse(Tensor<float> output, Size originSize, Padding padding)
    {
        var xRatio = (float)originSize.Width / YoloSegmentationOptions.Width;
        var yRatio = (float)originSize.Height / YoloSegmentationOptions.Height;

        var maxRatio = Math.Max(xRatio, yRatio);

        return Parse(output, originSize, padding, maxRatio);
    }

    private static IndexedBoundingBox[] Parse(Tensor<float> output, Size originSize, Padding padding , float maxRatio)
    {
        var boxes = new IndexedBoundingBox[output.Dimensions[2]];

        Parallel.For(0, output.Dimensions[2], detectedArea =>
        {
            for (var detectedClass = 0; detectedClass < YoloSegmentationOptions.Classes.Length; detectedClass++)
            {
                var confidence = output[0, detectedClass + 4, detectedArea];

                if (confidence <= YoloSegmentationOptions.ConfidenceThreshold)
                {
                    continue;
                }

                var areaBox = Rectangle.FromTensor(output, detectedArea);
                var bounds = ClampedBoundArea
                    .FromAreaInsideMaxRatio(areaBox, maxRatio, originSize)
                    .ToRectangle();

                var name = YoloSegmentationOptions.Classes[detectedClass];
                
                boxes[detectedArea] = new IndexedBoundingBox
                {
                    Index = detectedArea,
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

        return SelectRelevantBoxes(topBoxes);
    }

    private static IndexedBoundingBox[] SelectRelevantBoxes(IndexedBoundingBox[] boxes, float intersectionThreshold = 0.45f)
    {
        Array.Sort(boxes);

        var boxCount = boxes.Length;
        var activeCount = boxCount;
        var isNotActiveBoxes = new bool[boxCount];
        var selected = new List<IndexedBoundingBox>();

        for (var indexOfABox = 0; indexOfABox < boxCount; indexOfABox++)
        {
            if (isNotActiveBoxes[indexOfABox])
            {
                continue;
            }

            var boxA = boxes[indexOfABox];
            selected.Add(boxA);

            activeCount = DeactivateNonRelevantBoxes(
                boxes, 
                intersectionThreshold, 
                indexOfABox, 
                boxCount, 
                isNotActiveBoxes, 
                boxA, 
                activeCount);

            if (activeCount <= 0)
            {
                break;
            }
        }

        return [..selected];
    }

    private static int DeactivateNonRelevantBoxes(
        IndexedBoundingBox[] boxes, 
        float intersectionThreshold, 
        int startIndex,
        int boxCount, 
        bool[] isNotActiveBoxes, 
        IndexedBoundingBox boxA, 
        int activeCount)
    {
        for (var j = startIndex + 1; j < boxCount; j++)
        {
            if (isNotActiveBoxes[j])
            {
                continue;
            }

            var boxB = boxes[j];

            if (CalculateIntersection(boxA.Bounds, boxB.Bounds) < intersectionThreshold)
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

        return activeCount;
    }

    private static float CalculateIntersection(SixLabors.ImageSharp.Rectangle rectA, SixLabors.ImageSharp.Rectangle rectB)
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

        var intersectionArea = Area(SixLabors.ImageSharp.Rectangle.Intersect(rectA, rectB));
        var intersection = (float)intersectionArea / (areaA + areaB - intersectionArea);

        return intersection;
    }

    private static int Area(SixLabors.ImageSharp.Rectangle areaBox)
    {
        return areaBox.Width * areaBox.Height;
    }
}
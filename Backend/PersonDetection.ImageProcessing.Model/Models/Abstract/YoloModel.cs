using System.Collections.Generic;

namespace PersonDetection.ImageProcessing.Model.Models.Abstract;

public record YoloModel(
    int Width,
    int Height,
    int Depth,
    int Dimensions,
    int[] Strides,
    int[][][] Anchors,
    int[] Shapes,
    float Confidence,
    float MulConfidence,
    float Overlap,
    string[] Outputs,
    List<YoloLabel> Labels,
    bool UseDetect
);
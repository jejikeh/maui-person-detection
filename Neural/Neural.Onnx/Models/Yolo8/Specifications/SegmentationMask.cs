namespace Neural.Onnx.Models.Yolo8.Specifications;

public class SegmentationMask
{
    public required float[,] Mask { get; init; }
    
    public float this[int x, int y] => Mask[x, y];
}
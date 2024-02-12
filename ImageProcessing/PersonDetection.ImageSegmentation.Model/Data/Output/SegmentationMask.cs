namespace PersonDetection.ImageSegmentation.Model.Data.Output;

public class SegmentationMask
{
    public required float[,] Mask { get; init; }

    public float this[int x, int y] => Mask[x, y];

    public int Width => Mask.GetLength(0);

    public int Height => Mask.GetLength(1);
}
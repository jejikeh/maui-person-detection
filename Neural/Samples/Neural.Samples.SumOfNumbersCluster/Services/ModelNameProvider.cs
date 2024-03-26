namespace Neural.Samples.SumOfNumbersCluster.Services;

public class ModelNameProvider
{
    private static int _count = 0;
    
    public string GetModelName()
    {
        _count++;
        
        return _count.ToString();
    }
}
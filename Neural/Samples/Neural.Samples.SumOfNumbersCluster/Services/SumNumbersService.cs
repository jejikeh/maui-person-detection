namespace Neural.Samples.SumOfNumbersCluster.Services;

public class SumNumbersService
{
    public Task<int> SumAsync(IEnumerable<int> numbers)
    {
        return Task.FromResult(numbers.Sum());
    }
}
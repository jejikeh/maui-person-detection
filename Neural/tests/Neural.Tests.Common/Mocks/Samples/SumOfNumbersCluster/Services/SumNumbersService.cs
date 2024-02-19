namespace Neural.Tests.Common.Mocks.Samples.SumOfNumbersCluster.Services;

public class SumNumbersService
{
    public async Task<int> SumAsync(IEnumerable<int> numbers)
    {
        await Task.Delay(1000 * Random.Shared.Next(1, 3));
        
        return numbers.Sum();
    }
}
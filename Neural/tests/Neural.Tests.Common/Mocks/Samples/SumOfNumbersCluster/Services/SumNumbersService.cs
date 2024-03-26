namespace Neural.Tests.Common.Mocks.Samples.SumOfNumbersCluster.Services;

public class SumNumbersService
{
    public Task<int> SumAsync(IEnumerable<int> numbers)
    {
        return Task.FromResult(numbers.Sum());
    }
}
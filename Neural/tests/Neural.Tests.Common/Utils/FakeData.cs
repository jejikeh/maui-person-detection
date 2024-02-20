using Bogus;
using Neural.Tests.Common.Mocks.Models.Tasks;
using Neural.Tests.Common.Mocks.Samples.SumOfNumbersCluster.Tasks.IntToInt;

namespace Neural.Tests.Common.Utils;

public static class FakeData
{
    private static readonly Faker _faker = new Faker();
    
    public static string FilePath() => _faker.System.FilePath();
    
    public static int IntFromSmallRange() => _faker.Random.Int(5, 20);
    
    public static StringToStringTaskMock StringToStringTaskMock => new StringToStringTaskMock(_faker.System.FileName());

    public static IEnumerable<IntsToIntTask> IntsToIntTasks(int count)
    {
        var fakeTasks = new List<IntsToIntTask>();

        for (var i = 0; i < count; i++)
        {
            fakeTasks.Add(IntsToIntTask(count));
        }
        
        return fakeTasks;
    }
    
    public static IntsToIntTask IntsToIntTask(int count) => new IntsToIntTask(DigitsTo(count));

    public static int[] DigitsTo(int count) => Enumerable.Range(0, count).ToArray();
}
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
            fakeTasks.Add(IntsToIntTask());
        }
        
        return fakeTasks;
    }
    
    public static IntsToIntTask IntsToIntTask() => new IntsToIntTask(Digits());

    public static int[] Digits() => 
        _faker.Random.Digits(_faker.Random.Number(100));
}
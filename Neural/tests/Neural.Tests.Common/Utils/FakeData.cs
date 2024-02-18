using Bogus;
using Neural.Tests.Common.Mocks.Models.Tasks;

namespace Neural.Tests.Common.Utils;

public static class FakeData
{
    private static readonly Faker _faker = new Faker();
    
    public static string FilePath() => _faker.System.FilePath();
    
    public static int IntFromSmallRange() => _faker.Random.Int(5, 20);
    
    public static StringToStringTaskMock StringToStringTaskMock => new StringToStringTaskMock(_faker.System.FileName());
}